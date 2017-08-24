using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AttackLeague.AttackLeague
{
    public class Grid
    {
        private List<List<Tile>> myGrid;
        private List<AbstractBlock> myBlocks;
        private int myHeight = 12;
        private int myWidth = 6;
        private ContentManager myContent;
        private Vector2 myOffset = new Vector2(100, 100);
        private bool myPressedPrint = false;

        public Grid(ContentManager aContent)
        {
            myContent = aContent;
            GenerateGrid();
        }

        public void GenerateGrid()
        {
            myGrid = new List<List<Tile>>();
            myBlocks = new List<AbstractBlock>();

            for (int rows = 0; rows < myHeight; ++rows)
            {
                myGrid.Add(new List<Tile>());
                for (int columns = 0; columns < myWidth; ++columns)
                {
                    ColorBlock block = new ColorBlock();
                    block.SetPosition(columns, rows);
                    myBlocks.Add(block);

                    Tile tiley = new Tile();
                    tiley.SetBlock(block);
                    myGrid[rows].Add(tiley);                  
                }
            }

            foreach (AbstractBlock block in myBlocks)
            {
                block.LoadContent(myContent);
            }
        }

        public void Update()
        {
            //if it crashes here there are big chances that they have same position
            myBlocks.Sort();

            if (myPressedPrint == false && Keyboard.GetState().IsKeyDown(Keys.P))
            {
                myPressedPrint = true;
                PrintGrid();
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.P))
            {
                myPressedPrint = false;
            }
            
            for (int i = 0; i < myBlocks.Count; i++)
            {
                AbstractBlock block = myBlocks[i];
                block.Update();

                if (block is DisappearingBlock)
                {
                    if (((DisappearingBlock) block).IsAlive() == false)
                    {
                        EliminateBlock(i);
                    }
                }
                else if (block is FallingBlock)
                {
                    FallingBlock fallingBlock = (FallingBlock) block;
                    //Speed, might pass through many tiles?
                    if (fallingBlock.WillPassTile())
                    {
                        if (fallingBlock.GetPosition().Y == 0 ||
                            myGrid[fallingBlock.GetPosition().Y - 1][fallingBlock.GetPosition().X].IsEmpty() == false)
                        {
                            CreateBlock(fallingBlock.GetPosition(), new ColorBlock(fallingBlock.GetColor()));
                        }
                        else
                        {
                            fallingBlock.PassTile();
                            CreateBlock(fallingBlock.GetPosition() + new Point(0, 1), new EmptyBlock());

                            SetBlock(fallingBlock.GetPosition(), fallingBlock);
                        }
                    }

                }
                else if (block is ColorBlock)
                {
                    ColorBlock colorBlock = block as ColorBlock;
                    Rectangle blockRectangle = colorBlock.GetRectangle();

                    if (blockRectangle.Y != 0)
                    {
                        blockRectangle.Y--;
                        if (!RectangleIntersectsForFallingPurposes(blockRectangle))
                        {
                            Point position = colorBlock.GetPosition();
                            CreateBlock(position, new FallingBlock(colorBlock.GetColor()));
                       }
                    }
                }
            }

            CheckForMatches();
        }

        private bool RectangleIntersectsForFallingPurposes(Rectangle aRectangle)
        {
            for (int x = aRectangle.X; x < aRectangle.X + aRectangle.Width; x++)
            {
                for (int y = aRectangle.Y; y < aRectangle.Y + aRectangle.Height; y++)
                {

                    if (myGrid[y][x].CanFallThrough() == false )
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetBlock(Point aPosition, AbstractBlock aBlock)
        {
            aBlock.SetPosition(aPosition);

            AbstractBlock previousBlock = myGrid[aPosition.Y][aPosition.X].GetBlock();

            myBlocks[myBlocks.LastIndexOf(previousBlock)] = aBlock;

            myGrid[aPosition.Y][aPosition.X].SetBlock(aBlock);
        }

        //fixes everything
        private void CreateBlock(Point aPosition, AbstractBlock aBlock)
        {
            aBlock.SetPosition(aPosition);
            aBlock.LoadContent(myContent);

            AbstractBlock previousBlock = myGrid[aPosition.Y][aPosition.X].GetBlock();

            myBlocks[myBlocks.LastIndexOf(previousBlock)] = aBlock;

            myGrid[aPosition.Y][aPosition.X].SetBlock(aBlock);
        }

        private bool RectangleIntersects(Rectangle aRectangle)
        {
            for (int x = aRectangle.X; x < aRectangle.X + aRectangle.Width; x++)
            {
                for (int y = aRectangle.Y; y < aRectangle.Y + aRectangle.Height; y++)
                {
                    
                    if (myGrid[y][x].IsEmpty() == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void EliminateBlock(int aBlockIndex)
        {
            Point position = myBlocks[aBlockIndex].GetPosition();

            CreateBlock(position, new EmptyBlock());
        }

        private void CheckForMatches()
        {
            HashSet<AbstractBlock> matchedBlocks = new HashSet<AbstractBlock>();
            for (int i = myBlocks.Count - 1; i >= 0; --i)
            {
                AbstractBlock block = myBlocks[i];

                matchedBlocks.UnionWith(CheckMatches(block));
            }

            if (matchedBlocks.Count > 0 )
            {
                int i = 0;
                foreach (AbstractBlock blocky in matchedBlocks)
                {
                    //Console.WriteLine(blocky.GetPosition());
                    RemoveBlock(blocky, matchedBlocks.Count, i);
                    i++;
                }
            }
        }

        private HashSet<AbstractBlock> CheckMatches(AbstractBlock aBlock)
        {
            HashSet<AbstractBlock> matchingBlocks = new HashSet<AbstractBlock>();
            if (aBlock is ColorBlock )
            {
                ColorBlock block = (ColorBlock)aBlock;
                EBlockColor currentColor = block.GetColor();

                CheckMatchHorizontal(matchingBlocks, block, currentColor);

                CheckMatchVertical(matchingBlocks, block, currentColor);

            }
            return matchingBlocks;
        }

        /// <summary>
        /// Checks for matches to the ups
        /// </summary>
        private void CheckMatchVertical(HashSet<AbstractBlock> aMatchingBlocks, ColorBlock aBlock, EBlockColor aBlockColor)
        {
            Point blockPosition = aBlock.GetPosition();
            if (blockPosition.Y > 1)
            {
                AbstractBlock upBlock = GetBlockAtPosition(blockPosition.X, blockPosition.Y - 1);
                if (upBlock is ColorBlock && ((ColorBlock)upBlock).GetColor() == aBlockColor)
                {
                    AbstractBlock upUpBlock = GetBlockAtPosition(blockPosition.X, blockPosition.Y - 2);
                    if (upUpBlock is ColorBlock && ((ColorBlock)upUpBlock).GetColor() == aBlockColor)
                    {
                        aMatchingBlocks.Add(aBlock);
                        aMatchingBlocks.Add(upBlock);
                        aMatchingBlocks.Add(upUpBlock);
                    }
                }
            }
        }

        /// <summary>
        /// Checks for matches to the left
        /// </summary>
        private void CheckMatchHorizontal(HashSet<AbstractBlock> aMatchingBlocks, ColorBlock aBlock, EBlockColor aBlockColor)
        {
            Point blockPosition = aBlock.GetPosition();
            if (blockPosition.X > 1)
            {
                AbstractBlock leftBlock = GetBlockAtPosition(blockPosition.X - 1, blockPosition.Y);
                if (leftBlock is ColorBlock && ((ColorBlock)leftBlock).GetColor() == aBlockColor)
                {
                    AbstractBlock leftLeftBlock =
                        GetBlockAtPosition(blockPosition.X - 2, blockPosition.Y);
                    if (leftLeftBlock is ColorBlock && ((ColorBlock)leftLeftBlock).GetColor() == aBlockColor)
                    {
                        aMatchingBlocks.Add(aBlock);
                        aMatchingBlocks.Add(leftBlock);
                        aMatchingBlocks.Add(leftLeftBlock);
                    }
                }
            }
        }

        public void SwapRight(Point aPosition)
        {
            AbstractBlock leftBlock = GetBlockAtPosition(aPosition);
            AbstractBlock rightBlock = GetBlockAtPosition(aPosition + new Point(1, 0));

            if (leftBlock is ColorBlock || leftBlock is EmptyBlock)
            {
                if (rightBlock is ColorBlock ||
                    rightBlock is EmptyBlock)
                {
                    leftBlock.SetPosition(aPosition + new Point(1, 0));
                    rightBlock.SetPosition(aPosition);

                    myGrid[aPosition.Y][aPosition.X].SetBlock(rightBlock);
                    myGrid[aPosition.Y][aPosition.X + 1].SetBlock(leftBlock);
                }
            }
        }

        private AbstractBlock GetBlockAtPosition(Point aPosition)
        {
            return GetBlockAtPosition(aPosition.X, aPosition.Y);
        }

        private AbstractBlock GetBlockAtPosition(int aX, int aY)
        {
            if (aX < 0 || aX >= myWidth || aY < 0 || aY > myHeight)
            {
                return null;
            }

            return myGrid[aY][aX].GetBlock();
        }

        private void RemoveBlock(AbstractBlock aBlock, int aAmountOfDisappearingBlocks, int aCurrentBlock)
        {

            Point position = aBlock.GetPosition();

            int animationTime = 30;

            int delayBetweenAnimations = 15;

            int currentBlockDelta = aCurrentBlock * delayBetweenAnimations;

            int totalAnimationtime = aAmountOfDisappearingBlocks * delayBetweenAnimations + animationTime;

            int blockIndex = myBlocks.LastIndexOf(aBlock);

            if (blockIndex == -1)
            {
                return;
            }
            aBlock = new DisappearingBlock(((ColorBlock)aBlock).GetColorFromEnum(),
                totalAnimationtime,
                currentBlockDelta);

            CreateBlock(position, aBlock);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach (AbstractBlock iBlock in myBlocks)
            {
                iBlock.Draw(aSpriteBatch, myOffset, myHeight - 1, 0.0f);
            }
        }

        public Vector2 GetOffset()
        {
            return myOffset;
        }

        public int GetHeight()
        {
            return myHeight;
        }

        void PrintGrid()
        {
            for (int iRow = myHeight - 1; iRow >= 0; iRow--)
            {
                for (int iColumn = 0; iColumn < myWidth; ++iColumn)
                {
                    string color = "X";
                    AbstractBlock blocky = myGrid[iRow][iColumn].GetBlock();
                    if (blocky is ColorBlock)
                    {
                        ColorBlock colorBlock = (ColorBlock)blocky;
                        switch (colorBlock.GetColor())
                        {
                            case EBlockColor.Blue:
                                color = "B";
                                break;
                            case EBlockColor.Cyan:
                                color = "C";
                                break;
                            case EBlockColor.Green:
                                color = "G";
                                break;
                            case EBlockColor.Magenta:
                                color = "M";
                                break;
                            case EBlockColor.Red:
                                color = "R";
                                break;
                            case EBlockColor.Yellow:
                                color = "Y";
                                break;
                        }


                    }
                    Console.Write(color + " ");
                }
                Console.Write("\n");
            }
        }

        public void RaiseBlocks()
        {
            //DO DA RAISINS
        }
    }
}
