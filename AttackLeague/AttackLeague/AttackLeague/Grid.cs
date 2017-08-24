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
           

            for (int i = myBlocks.Count - 1; i >= 0; --i)
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
                    
                }
            }

            CheckForMatches();
        }

        private void EliminateBlock(int aBlockIndex)
        {
            Point position = myBlocks[aBlockIndex].GetPosition();

            myBlocks[aBlockIndex] = new EmptyBlock();

            myBlocks[aBlockIndex].SetPosition(position.X, position.Y);

            myGrid[position.Y][position.X].SetBlock(myBlocks[aBlockIndex]);
        }

        private void CheckForMatches()
        {
            List<AbstractBlock> matchedBlocks = new List<AbstractBlock>();
            for (int i = myBlocks.Count - 1; i >= 0; --i)
            {
                AbstractBlock block = myBlocks[i];

                matchedBlocks.AddRange(CheckMatches(block));
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

        private List<AbstractBlock> CheckMatches(AbstractBlock aBlock)
        {
            List<AbstractBlock> matchingBlocks = new List<AbstractBlock>();
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
        private void CheckMatchVertical(List<AbstractBlock> aMatchingBlocks, ColorBlock aBlock, EBlockColor aBlockColor)
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
        private void CheckMatchHorizontal(List<AbstractBlock> aMatchingBlocks, ColorBlock aBlock, EBlockColor aBlockColor)
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

        //private int GetIndexFromBlock(AbstractBlock aBlock)
        //{
        //    return GetIndexFromPosition(aBlock.GetPosition());
        //}

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


        //private int GetIndexFromPosition(Point aPosition)
        //{
        //    return GetIndexFromPosition(aPosition.X, aPosition.Y);
        //}

        //private int GetIndexFromPosition(int aX, int aY)
        //{
        //    return aX + aY * myWidth;
        //}

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
            aBlock = myBlocks[blockIndex] = new DisappearingBlock(((ColorBlock)aBlock).GetColorFromEnum(),
                totalAnimationtime,
                currentBlockDelta);
            myGrid[position.Y][position.X].SetBlock(myBlocks[blockIndex]);

            aBlock.SetPosition(position.X, position.Y);
            aBlock.LoadContent(myContent);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach (AbstractBlock iBlock in myBlocks)
            {
                iBlock.Draw(aSpriteBatch, myOffset, myHeight - 1);
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
    }
}
