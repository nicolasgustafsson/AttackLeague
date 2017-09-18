using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;
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
        private float myRaisingOffset = 0f;
        private Sprite myBorderSprite;
        private SpriteFont myFont;

        //Super ugly pls fix
        private bool myHasRaisedThisFrame = false;
        private bool myWantsToRaiseBlocks = false;
        private bool myIsPaused = false;
        private bool myHasStarted = true; // should be false inherently
        private float myChainTimer = 0f;
        private float myGameTime = 0f;
        private float myAdditionalGameSpeed = 0.0f;
        private float myGameSpeed = 1.0f;

        private const float MyConstantRaisingSpeed = 3;

        private const float MyMaxMercyTimer = 3.0f;
        private float myMercyTimer = MyMaxMercyTimer;

        private int myChainCounter = 1;

        public Grid(ContentManager aContent)
        {
            myContent = aContent;
            Utility.FrameCounter.ResetFrames();
            GenerateGrid();

            myBorderSprite = new Sprite("GridBorder", aContent);

            myFont = aContent.Load<SpriteFont>("raditascartoon");

            ActionMapper.BindAction("IncreaseGameSpeed", Keys.T, KeyStatus.KeyPressed);
        }

        public void GenerateGrid()
        {
            myGrid = new List<List<Tile>>();
            myBlocks = new List<AbstractBlock>();

            myGrid.Add(new List<Tile>());
            for (int columns = 0; columns < myWidth; ++columns)
            {
                FrozenBlock block = new FrozenBlock();
                block.SetPosition(columns, 0);
                myBlocks.Add(block);

                Tile tiley = new Tile();
                myGrid[0].Add(tiley);
                tiley.SetBlock(block);
            }

            for (int rows = 1; rows < myHeight; ++rows)
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

            PrintGrid();
        }

        public void SetIsRaisingBlocks()
        {
            if (BlocksAreBusy() == false)
                myWantsToRaiseBlocks = true;
        }

        private void RearrangeRaisedTiles()
        {
            for (int rows = myGrid.Count() -1; rows >= 1; rows--)
            {
                if (RowIsEmpty(rows) == false)
                {
                    if (HasRow(rows + 1) == false)
                    {
                        AddEmptyRow();
                        //MoveRowUp(rows);
                        //rows++;
                        //continue;
                    }
                    
                    MoveRowUp(rows);
                }
            }
            ConvertFrozenRowToColorBlocks();
            CreateFrozenRow();
            PrintGrid();
            myWantsToRaiseBlocks = false;
        }

        private void CreateFrozenRow()
        {
            for (int columns = 0; columns < myWidth; ++columns)
            {
                AbstractBlock blocky = myGrid[0][columns].GetBlock();
                Debug.Assert(blocky is FrozenBlock);
                ((FrozenBlock)(myGrid[0][columns].GetBlock())).RandomizeColor();
            }
        }

        private void ConvertFrozenRowToColorBlocks()
        {
            for (int column = 0; column < myWidth; column++)
            {
                AbstractBlock blocky = myGrid[0][column].GetBlock();
                Debug.Assert(blocky is FrozenBlock);
                ColorBlock colorBlocky = new ColorBlock(((FrozenBlock)blocky).GetColor());
                colorBlocky.SetPosition(new Point(column, 1));
                Tile tiley = new Tile();
                tiley.SetBlock(colorBlocky);

                myGrid[1][column] = tiley;
                myBlocks.Add(colorBlocky);
                colorBlocky.LoadContent(myContent);

            }
        }

        private void MoveRowUp(int aRowNumber)
        {
            for (int columns = 0; columns < myWidth; ++columns)
            {
                myGrid[aRowNumber + 1][columns] = myGrid[aRowNumber][columns];
                myGrid[aRowNumber + 1][columns].GetBlock().SetPosition(columns, aRowNumber + 1);
            }
        }

        private void AddEmptyRow()
        {
            myGrid.Add(new List<Tile>());
            int row = myGrid.Count() - 1;
            for (int columns = 0; columns < myWidth; ++columns)
            {
                EmptyBlock block = new EmptyBlock();
                block.SetPosition(columns, row);
                myBlocks.Add(block);

                Tile tiley = new Tile();
                tiley.SetBlock(block);
                myGrid[row].Add(tiley);
            }
        }

        private bool RowIsEmpty(int aRowNumber)
        {
            for (int columns = 0; columns < myWidth; columns++)
            {
                if ((myGrid[aRowNumber][columns].GetBlock() is EmptyBlock) == false)
                    return false;
            }
            return true;
        }

        private bool HasRow(int aRowNumber)
        {
            return myGrid.Count() - 1 > aRowNumber;
        }

        public void Update() //HEREISUPDATE ----------------------------------------------------------------------------------------------------------
        {
            if (ActionMapper.ActionIsActive("IncreaseGameSpeed"))
                myAdditionalGameSpeed += 0.5f;
            myHasRaisedThisFrame = false;
            Utility.FrameCounter.IncrementFrameCount();

            const float DeltaTime = 1.0f / 60.0f;
            if (myIsPaused == false && myHasStarted == true)
            {
                myGameTime += DeltaTime;
                myGameSpeed = Math.Min(1.0f + (myGameTime / 60f) + myAdditionalGameSpeed, 10f);
            }

            if (!BlocksAreBusy())
                myChainTimer -= DeltaTime * myGameSpeed;

            if (myWantsToRaiseBlocks == true)
                myChainTimer = 0f;

            if (IsFrozen() == false)
            {
                if (myChainCounter > 1)
                {
                    OnChainEnd(myChainCounter);
                    myChainCounter = 1;
                }
            }

            if (IsFrozen() == false && myChainTimer <= 0f)
            {
                if (IsExceedingRoof() == true)
                {
                    myMercyTimer -= DeltaTime * myGameSpeed;
                    if (myMercyTimer < 0)
                    {
                        Lose();
                    }
                }
                else
                {
                    float tilesPerSecond = 0.3f * myGameSpeed;

                    if (myWantsToRaiseBlocks && MyConstantRaisingSpeed > tilesPerSecond)
                    {
                        tilesPerSecond = MyConstantRaisingSpeed;
                    }

                    myRaisingOffset -= (tilesPerSecond / AbstractBlock.GetTileSize());

                    if (RaisingOffsetExceededTile() == true)
                    {
                        RearrangeRaisedTiles();
                        myRaisingOffset += 1f;
                        myHasRaisedThisFrame = true;
                    }

                    //reset mercy timer
                    myMercyTimer = MyMaxMercyTimer / myGameSpeed;
                }
            }


            //if it crashes here there are big chances that they have same position
            myBlocks.Sort();

            if (KeyboardWrapper.KeyPressed(Keys.P))
            { 
                PrintGrid();
            }
            
            for (int i = 0; i < myBlocks.Count; i++)
            {
                AbstractBlock block = myBlocks[i];
                block.Update(myGameSpeed);

                if (block is DisappearingBlock)
                {
                    if (((DisappearingBlock) block).IsAlive() == false)
                    {
                        EliminateBlock(i);

                        //Get tile position
                        Point blockPosition = block.GetPosition();
                        int column = blockPosition.X;

                        for (int row = blockPosition.Y +1; row < myGrid.Count; row++)
                        {
                            AbstractBlock becomeFallingBlock = myGrid[row][column].GetBlock();
                            if (becomeFallingBlock is ColorBlock colorBecomeFallingBlock)
                            {
                                colorBecomeFallingBlock.CanChain = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else if (block is FallingBlock fallingBlock)
                {
                    //Speed, might pass through many tiles?
                    if (fallingBlock.WillPassTile(myGameSpeed))
                    {
                        if (fallingBlock.GetPosition().Y == 0 ||
                            myGrid[fallingBlock.GetPosition().Y - 1][fallingBlock.GetPosition().X].IsEmpty() == false)
                        {
                            CreateBlock(fallingBlock.GetPosition(), new ColorBlock(fallingBlock));
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
                        if (RectangleIntersectsForFallingPurposes(blockRectangle) == false)
                        {
                            Point position = colorBlock.GetPosition();
                            CreateBlock(position, new FallingBlock(colorBlock));
                       }
                    }
                }
            }

            CheckForMatches();

            ResetCanChain();
        }

        private void OnChainIncrement(int ChainLength)
        {
            Console.WriteLine($"Chain {ChainLength}!");
            myChainTimer = Math.Max(1.0f, myChainTimer + 1.0f);
        }

        private void OnChainEnd(int ChainLength)
        {
            Console.WriteLine($"Chain ended at {ChainLength}!");
        }

        private void ResetCanChain()
        {
            foreach(AbstractBlock block in myBlocks)
            {
                if (block is ColorBlock colorBlock)
                {
                    colorBlock.CanChain = false;
                }
            }
        }

        private void Lose()
        {
            Console.WriteLine("Thou art Losar");
        }

        private bool IsExceedingRoof()
        {
            if (myGrid.Count() > myHeight)
            {
                for (int column = 0; column < myWidth; column++)
                {
                    if (myGrid[myHeight][column].GetBlock() is EmptyBlock == false)
                        return true;
                }
            }
            return false;
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
            bool hasChained = false;
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
                    if (blocky is ColorBlock colorBlocky)
                    {
                        if (colorBlocky.CanChain == true)
                        {
                            hasChained = true;
                        }
                    }
                    //Console.WriteLine(blocky.GetPosition());
                    RemoveBlock(blocky, matchedBlocks.Count, i);
                    i++;
                }
            }

            if (matchedBlocks.Count > 3)
            {
                OnCombo(matchedBlocks);
            }

            if (hasChained)
            {
                myChainCounter++;
                OnChainIncrement(myChainCounter);
            }
        }

        private static void OnCombo(HashSet<AbstractBlock> matchedBlocks)
        {
            
            Console.WriteLine($"Combo! {matchedBlocks.Count}");
        }

        private HashSet<AbstractBlock> CheckMatches(AbstractBlock aBlock)
        {
            HashSet<AbstractBlock> matchingBlocks = new HashSet<AbstractBlock>();
            if (aBlock is ColorBlock block)
            {
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
            aBlock = new DisappearingBlock(((AbstractColorBlock)aBlock).GetColor(),
                totalAnimationtime,
                currentBlockDelta);

            CreateBlock(position, aBlock);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach (AbstractBlock iBlock in myBlocks)
            {
                iBlock.Draw(aSpriteBatch, myOffset, myHeight - 1, myRaisingOffset);
            }

            myBorderSprite.SetPosition(new Vector2(myOffset.X - 2, 6 - AbstractBlock.GetTileSize()));
            myBorderSprite.Draw(aSpriteBatch);

            aSpriteBatch.DrawString(myFont, 
                "Bonus time: " + myChainTimer.ToString() + 
                "\nGame Time: " + myGameTime.ToString() + 
                "\nGame Speed: " + myGameSpeed.ToString()
                , new Vector2(500, 100), Color.MidnightBlue);
        }

        public Vector2 GetOffset()
        {
            return myOffset;
        }

        public int GetHeight()
        {
            return myHeight;
        }

        public float GetRaisingOffset()
        {
            return myRaisingOffset;
        }

        public bool IsFrozen()
        {
            if (myChainTimer > 0f)
                return true;

            return BlocksAreBusy();
        }

        private bool BlocksAreBusy()
        {
            foreach (AbstractBlock blocky in myBlocks)
            {
                if (blocky is FallingBlock || blocky is DisappearingBlock)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetWidth()
        {
            return myWidth;
        }

        public bool HasRaisedGridThisFrame()
        {
            return myHasRaisedThisFrame;
        }

        private bool RaisingOffsetExceededTile()
        {
            return myRaisingOffset < -1f;
        }

        void PrintGrid()
        {
            for (int iRow = myHeight - 1; iRow >= 0; iRow--)
            {
                for (int iColumn = 0; iColumn < myWidth; ++iColumn)
                {
                    string color = "X";
                    AbstractBlock blocky = myGrid[iRow][iColumn].GetBlock();
                    if (blocky is ColorBlock colorBlock)
                    {
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
            Console.Write("\n--\n");
        }
    }
}
