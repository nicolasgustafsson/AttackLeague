﻿using AttackLeague.AttackLeague.Blocks.Generator;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Grid
{
    class GridBehavior
    {
        private BlockGenerator myBlockGenerator;
        private GridContainer myGridContainer;

        private Vector2 myOffset = new Vector2(100, 100);
        private Sprite myBorderSprite;
        private SpriteFont myFont;

        private bool myIsPaused = false;
        private bool myHasStarted = true; // should be false inherently
        private float myChainTimer = 0f;
        private float myGameTime = 0f;
        private float myAdditionalGameSpeed = 0.0f;
        private float myGameSpeed = 1.0f;

        private const float MyMaxMercyTimer = 3.0f;
        private float myMercyTimer = MyMaxMercyTimer;

        private bool myHasRaisedThisFrame = false;
        private bool myWantsToRaiseBlocks = false;
        private float myRaisingOffset = 0f;
        private const float MyConstantRaisingSpeed = 3;

        private int myChainCounter = 1;

        public GridBehavior(/*BlockFactory aBlockFactory, GridContainer aGridContainer,*/ int aPlayerIndex)
        {
            myGridContainer = new GridContainer();
            myBlockGenerator = myGridContainer.GetBlockFactory();

            myBlockGenerator.GenerateGrid();
            myBorderSprite = new Sprite("GridBorder");
            myFont = ContentManagerInstance.Content.Load<SpriteFont>("raditascartoon");

            // set grid position based on how many players there are
            Debug.Assert(GameInfo.GameInfo.myPlayerCount > 0);
            myOffset.X = (GameInfo.GameInfo.myScreenSize.X / (GameInfo.GameInfo.myPlayerCount + 1)) * (aPlayerIndex + 1);
            myOffset.X -= myBorderSprite.GetSize().X / 2f;
        }

        public void OnGridReset()
        {
            myChainTimer = 0;
            myChainCounter = 0;
            myMercyTimer = MyMaxMercyTimer;
        }

        public void SetIsRaisingBlocks()
        {
            if (BlocksAreBusy() == false)
                myWantsToRaiseBlocks = true;
        }

        private void HandleChains(float aDeltaTime)
        {
            if (!BlocksAreBusy())
                myChainTimer -= aDeltaTime * myGameSpeed;

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
        }

        // move to disappearing block
        private void UpdateDisappearingBlock(DisappearingBlock aBlock, int aIndex)
        {
            if (aBlock.IsAlive() == false)
            {
                myGridContainer.EliminateBlock(aIndex);//[TODO] run this on blockfactory

                //Get tile position
                Point blockPosition = aBlock.GetPosition();
                int column = blockPosition.X;

                for (int row = blockPosition.Y + 1; row < myGridContainer.myGrid.Count; row++)
                {
                    AbstractBlock becomeFallingBlock = myGridContainer.myGrid[row][column].GetBlock();
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

        // move to falling block
        private void UpdateFallingBlock(FallingBlock aBlock)
        {
            //Speed, might pass through many tiles?
            if (aBlock.WillPassTile(myGameSpeed))
            {
                if (aBlock.GetPosition().Y == 0 ||
                    myGridContainer.myGrid[aBlock.GetPosition().Y - 1][aBlock.GetPosition().X].IsEmpty() == false)
                {
                    myGridContainer.InitializeBlock(aBlock.GetPosition(), new ColorBlock(aBlock)); // MOVE TO FACTORY I GUESS, or blocks who tell factory
                }
                else
                {
                    aBlock.PassTile();
                    myGridContainer.InitializeBlock(aBlock.GetPosition() + new Point(0, 1), new EmptyBlock());

                    myGridContainer.SetBlock(aBlock.GetPosition(), aBlock);
                }
            }
        }

        // move to abstract color block
        private void UpdateAbstractColorBlock(AbstractColorBlock aBlock)
        {
            aBlock.UpdateIconAnimation(ColumnIsExceedingRoof(aBlock.GetPosition().X), ColumnIsCloseToExceedingRoof(aBlock.GetPosition().X));
        }

        // move to colorblock
        private void UpdateColorBlock(ColorBlock aBlock)
        {
            Rectangle blockRectangle = aBlock.GetRectangle();

            if (blockRectangle.Y != 0)
            {
                blockRectangle.Y--;
                if (RectangleIntersectsForFallingPurposes(blockRectangle) == false)
                {
                    Point position = aBlock.GetPosition();
                    myGridContainer.InitializeBlock(position, new FallingBlock(aBlock));
                }
            }
        }

        public void Update() //HEREISUPDATE ----------------------------------------------------------------------------------------------------------
        {
            myHasRaisedThisFrame = false;

            const float DeltaTime = 1.0f / 60.0f;
            if (myIsPaused == false && myHasStarted == true)
            {
                myGameTime += DeltaTime;
                myGameSpeed = Math.Min(1.0f + (myGameTime / 60f) + myAdditionalGameSpeed, 10f);
            }

            HandleChains(DeltaTime);

            if (IsFrozen() == false && myChainTimer <= 0f)
            {
                if (IsExceedingRoof() == true)
                {
                    myMercyTimer -= DeltaTime * myGameSpeed;
                    if (myMercyTimer < 0)
                        Lose();
                }
                else
                {
                    float tilesPerSecond = 0.3f * myGameSpeed;
                    Raise(tilesPerSecond);
                    //reset mercy timer
                    myMercyTimer = MyMaxMercyTimer / myGameSpeed;
                }
            }

            //if it crashes here there are big chances that they have same position
            myGridContainer.myBlocks.Sort();

            // just update all blocks, give them an update function to override
            for (int i = 0; i < myGridContainer.myBlocks.Count; i++)
            {
                AbstractBlock block = myGridContainer.myBlocks[i];
                block.Update(myGameSpeed);

                if (block is AbstractColorBlock abstractColorBlock)
                    UpdateAbstractColorBlock(abstractColorBlock);

                if (block is DisappearingBlock disappearingBlock)
                    UpdateDisappearingBlock(disappearingBlock, i);
                else if (block is FallingBlock fallingBlock)
                    UpdateFallingBlock(fallingBlock);
                else if (block is ColorBlock colorBlock && block.IsSwitching() == false)
                    UpdateColorBlock(colorBlock);
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
            foreach (AbstractBlock block in myGridContainer.myBlocks)
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

        // duplicated in gridcontainer?
        private bool RectangleIntersectsForFallingPurposes(Rectangle aRectangle)
        {
            for (int x = aRectangle.X; x < aRectangle.X + aRectangle.Width; x++)
            {
                for (int y = aRectangle.Y; y < aRectangle.Y + aRectangle.Height; y++)
                {
                    if (myGridContainer.myGrid[y][x].CanFallThrough() == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void CheckForMatches()
        {
            bool hasChained = false;
            HashSet<AbstractBlock> matchedBlocks = new HashSet<AbstractBlock>();
            for (int i = myGridContainer.myBlocks.Count - 1; i >= 0; --i)
            {
                AbstractBlock block = myGridContainer.myBlocks[i];

                matchedBlocks.UnionWith(CheckMatches(block));
            }

            if (matchedBlocks.Count > 0)
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
                    myBlockGenerator.RemoveBlock(blocky, matchedBlocks.Count, i);
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

        private void OnCombo(HashSet<AbstractBlock> matchedBlocks)
        {
            Console.WriteLine($"Combo! {matchedBlocks.Count}");
            const float MagicNumber = 0.3f;
            if (myChainTimer < 0.0f)
                myChainTimer = 0.0f;
            myChainTimer += (matchedBlocks.Count * MagicNumber) / myGameSpeed;
        }

        private HashSet<AbstractBlock> CheckMatches(AbstractBlock aBlock)
        {
            HashSet<AbstractBlock> matchingBlocks = new HashSet<AbstractBlock>();
            if (aBlock is ColorBlock block)
            {
                CheckMatchesDirection(block, new Point(1, 0), matchingBlocks);
                CheckMatchesDirection(block, new Point(0, 1), matchingBlocks);
            }
            return matchingBlocks;
        }

        private void CheckMatchesDirection(ColorBlock aBlock, Point aOffset, HashSet<AbstractBlock> aMatchingBlocks)
        {
            Point blockPosition = aBlock.GetPosition();
            Point offsetPosition = blockPosition + aOffset;
            AbstractBlock directionBlock = myGridContainer.GetBlockAtPosition(offsetPosition.X, offsetPosition.Y);
            if (directionBlock is ColorBlock && ((ColorBlock)directionBlock).GetColor() == aBlock.GetColor() &&
                directionBlock.IsSwitching() == false && aBlock.IsSwitching() == false)
            {
                Point offsetOffsetPosition = blockPosition + new Point(aOffset.X * 2, aOffset.Y * 2);
                AbstractBlock directionDirectionBlock = myGridContainer.GetBlockAtPosition(offsetOffsetPosition.X, offsetOffsetPosition.Y);
                if (directionDirectionBlock is ColorBlock && ((ColorBlock)directionDirectionBlock).GetColor() == aBlock.GetColor() &&
                    directionDirectionBlock.IsSwitching() == false)
                {
                    aMatchingBlocks.Add(aBlock);
                    aMatchingBlocks.Add(directionBlock);
                    aMatchingBlocks.Add(directionDirectionBlock);
                }
            }
        }

        public void DebugRerandomizeGrid()
        {
            myGridContainer.DebugRerandomizeGrid();
        }

        public void SwapBlocksRight(Point aPosition)
        {
            AbstractBlock leftBlock = myGridContainer.GetBlockAtPosition(aPosition);
            AbstractBlock rightBlock = myGridContainer.GetBlockAtPosition(aPosition + new Point(1, 0));

            if (leftBlock is ColorBlock || leftBlock is EmptyBlock)
            {
                if (rightBlock is ColorBlock ||
                    rightBlock is EmptyBlock)
                {
                    leftBlock.SetPosition(aPosition + new Point(1, 0));
                    rightBlock.SetPosition(aPosition);

                    myGridContainer.myGrid[aPosition.Y][aPosition.X].SetBlock(rightBlock);
                    myGridContainer.myGrid[aPosition.Y][aPosition.X + 1].SetBlock(leftBlock);

                    float switchTime = 1.0f / myGameSpeed;
                    leftBlock.DoTheSwitchingCalculating(switchTime, ESwitchDirection.ToTheRight);
                    rightBlock.DoTheSwitchingCalculating(switchTime, ESwitchDirection.ToTheLeft);
                }
            }
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach (AbstractBlock iBlock in myGridContainer.myBlocks)
            {
                iBlock.Draw(aSpriteBatch, myOffset, myGridContainer.GetHeight() - 1, myRaisingOffset);
            }

            myBorderSprite.SetPosition(new Vector2(myOffset.X - 2, 6 - AbstractBlock.GetTileSize()));
            myBorderSprite.Draw(aSpriteBatch);

            //aSpriteBatch.DrawString(myFont, 
            //    "Bonus time: " + myChainTimer.ToString() + 
            //    "\nGame Time: " + myGameTime.ToString() + 
            //    "\nGame Speed: " + myGameSpeed.ToString()
            //    , new Vector2(500, 100), Color.MidnightBlue);
        }

        public void AddGameSpeed(float aAmount)
        {
            myAdditionalGameSpeed += aAmount; // 0.5f good value
        }

        public Vector2 GetOffset()
        {
            return myOffset;
        }

        public bool IsFrozen()
        {
            if (myChainTimer > 0f)
                return true;

            return BlocksAreBusy();
        }

        private bool BlocksAreBusy()
        {
            foreach (AbstractBlock blocky in myGridContainer.myBlocks)
            {
                if (blocky is FallingBlock || blocky is DisappearingBlock)
                {
                    return true;
                }
            }
            return false;
        }

        private void RearrangeRaisedTiles()
        {
            myBlockGenerator.RearrangeRaisedTiles();
            //myGridContainer.PrintGrid();
            myWantsToRaiseBlocks = false; // is this a hack?
        }

        public void Raise(float RaiseAmount)
        {
            if (myWantsToRaiseBlocks && MyConstantRaisingSpeed > RaiseAmount)
            {
                RaiseAmount = MyConstantRaisingSpeed;
            }

            myRaisingOffset -= (RaiseAmount / AbstractBlock.GetTileSize());

            if (RaisingOffsetExceededTile() == true)
            {
                RearrangeRaisedTiles();
                myRaisingOffset += 1f;
                myHasRaisedThisFrame = true;
            }
        }

        public float GetRaisingOffset()
        {
            return myRaisingOffset;
        }

        private bool RaisingOffsetExceededTile()
        {
            return myRaisingOffset < -1f;
        }

        public bool HasRaisedGridThisFrame()
        {
            return myHasRaisedThisFrame;
        }

        public int GetWidth()
        {
            return myGridContainer.GetWidth();
        }

        public int GetHeight()
        {
            return myGridContainer.GetHeight();
        }

        // could be moved to gridcontainer
        private bool IsExceedingRoof()
        {
            for (int column = 0; column < myGridContainer.GetWidth(); column++)
            {
                if (ColumnIsExceedingRoof(column) == true)
                    return true;
            }

            return false;
        }

        // could be moved to grid container
        public bool ColumnIsExceedingRoof(int aColumn)
        {
            if (myGridContainer.myGrid.Count() <= myGridContainer.GetHeight())
                return false;
            if (myGridContainer.myGrid[myGridContainer.GetHeight()][aColumn].GetBlock() is EmptyBlock == false)
                return true;
            return false;
        }

        // could be moved to grid container
        public bool ColumnIsCloseToExceedingRoof(int aColumn)
        {
            if (myGridContainer.myGrid.Count() <= myGridContainer.GetHeight() - 2)
                return false;
            if (myGridContainer.myGrid[myGridContainer.GetHeight() - 2][aColumn].GetBlock() is EmptyBlock == false)
                return true;
            return false;
        }
    }
}