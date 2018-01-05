using AttackLeague.AttackLeague.Blocks;
using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.AttackLeague.Blocks.Generator;
using AttackLeague.AttackLeague.Feedback;
using AttackLeague.Utility;
using AttackLeague.Utility.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AttackLeague.AttackLeague.Grid
{
    public class GridBehavior
    {
        /*
         TODO!
            BUGS:
             Disintegrates several blockbundles at once(create blockiterator which can execute function at block with a delay
             Rightmost block gets converted first on last row
             Goes left to right(right to left)
             Crashes sometimes(AllowFalling?) in loopy thingy, index out of bounds I think

            Notes for Tuesday!
                Do the BlockDelegate function and use it in the non-compiling code below
                Steal functionality from AngryBlockBundle (solve the things with updating into frozen things)
                see what needs to be done!

             Yes.
             then have some hot cocoa.
             Yes.

            YLF TAKE THE WEEEL
         
         Perhaps fiddle around with SpriteTileset 
        */

        private int myPlayerIndex;

        private BlockGenerator myBlockGenerator;
        private GridContainer myGridContainer;

        private List<AngryBlockBundle> myAngryBundles;
        private List<BlockTimedIterator> myBlockIterators;

        private Vector2 myOffset = new Vector2(100, 100);
        private Sprite myBorderSprite;
        private SpriteFont myFont;

        private bool myIsPaused = false;
        private bool myHasStarted = true; // should be false inherently
        private float myChainTimer = 0f;
        private float myGameTime = 0f;
        private float myAdditionalGameSpeed = 0.0f;
        private float myGameSpeed = 1.0f;
        private float myModifiedGameSpeed = 0.0f;
        private float myRaisingSpeed { get { return myGameSpeed * 4f; } } // magic tweaking!

        private const float MyMaxMercyTimer = 3.0f;
        private float myMercyTimer = MyMaxMercyTimer;

        private bool myHasRaisedThisFrame = false;
        private bool myWantsToRaiseBlocks = false;
        private float myRaisingOffset = 0f;
        private const float MyManualRaisingSpeed = 3;//0
        public bool myIsDead { get;  private set; }

        private int myChainCounter = 1;

        public GridBehavior(GridContainer aGridContainer, int aPlayerIndex)
        {
            myPlayerIndex = aPlayerIndex;
            myIsDead = false;
            myAngryBundles = new List<AngryBlockBundle>();

            myGridContainer = aGridContainer;
            myBlockGenerator = myGridContainer.GetBlockGenerator();

            myBlockGenerator.GenerateGrid();
            myBorderSprite = new Sprite("GridBorder");
            myFont = ContentManagerInstance.Content.Load<SpriteFont>("raditascartoon");

            // set grid position based on how many players there are
            Debug.Assert(GameInfo.GameInfo.myPlayerCount > 0);
            myOffset.X = (GameInfo.GameInfo.myScreenSize.X / (GameInfo.GameInfo.myPlayerCount + 1)) * (myPlayerIndex + 1);
            myOffset.X -= myBorderSprite.GetSize().X / 2f;
            myBlockIterators = new List<BlockTimedIterator>();
        }

        public void OnGridReset()
        {
            myChainTimer = 0;
            myChainCounter = 0;
            myMercyTimer = MyMaxMercyTimer;
            myIsDead = false;
            myBlockIterators.Clear();
            foreach (var angryyy in myAngryBundles)
            {
                angryyy.OnDestroy();
            }
            myAngryBundles.Clear();
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

            if (BlocksAreBusy() == false)
            {
                if (myChainCounter > 1)
                {
                    OnChainEnd(myChainCounter);
                    myChainCounter = 1;
                }
            }
        }

        public void OnBlockEliminated(Point aPosition)
        {
            //Get tile position
            int column = aPosition.X;

            for (int row = aPosition.Y + 1; row < myGridContainer.myGrid.Count; row++)
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

        public void Update() //HEREISUPDATE ----------------------------------------------------------------------------------------------------------
        {
            myHasRaisedThisFrame = false;

            const float DeltaTime = 1.0f / 60.0f;
            if (myIsPaused == false && myHasStarted == true)
            {
                myGameTime += DeltaTime;
                myGameSpeed = Math.Min(1.0f + (myGameTime / 60f) + myAdditionalGameSpeed, 10f);
                myModifiedGameSpeed = (float)Math.Log10(myGameSpeed +1);
            }

            HandleChains(DeltaTime);

            if (IsFrozen() == false && myChainTimer <= 0f)
            {
                if (myGridContainer.IsExceedingRoof() == true)
                {
                    myMercyTimer -= DeltaTime * myModifiedGameSpeed;
                    if (myMercyTimer < 0)
                    {
                        myIsDead = true;
                        DeadFeedback();
                    }
                }
                else
                {
                    float tilesPerSecond = myModifiedGameSpeed;
                    Raise(tilesPerSecond);
                    //reset mercy timer
                    myMercyTimer = MyMaxMercyTimer / myModifiedGameSpeed;
                }
            }

            //if it crashes here there are big chances that they have same position
            myGridContainer.myBlocks.Sort();
            foreach (var angryBundle in myAngryBundles)
            {
                angryBundle.UpdateFallingStatus();
            }

            // just update all blocks, give them an update function to override
            for (int i = 0; i < myGridContainer.myBlocks.Count; i++)
            {
                AbstractBlock block = myGridContainer.myBlocks[i];
                block.Update(myModifiedGameSpeed);
            }
            foreach (var angryBundle in myAngryBundles) // foreach bundle if bundle isDisitergraintigb UpdateDIsintegrate break;
            {
                angryBundle.Update(myModifiedGameSpeed);
            }
            for (int i = 0; i < myBlockIterators.Count; ++i)
            {
                myBlockIterators[i].Update();
                if (myBlockIterators[i].IsFinished())
                {
                    myBlockIterators.RemoveAt(i);
                    i--;
                }
            }

            CheckForMatches();
            ResetCanChain();

            for (int iBundle = 0; iBundle < myAngryBundles.Count(); iBundle++)
            {
                if (myAngryBundles[iBundle].IsDed())
                {
                    myAngryBundles[iBundle].OnDestroy();
                    myAngryBundles.RemoveAt(iBundle);
                    iBundle--;
                }
            }

            myGridContainer.RemoveTopRows();
        }

        private void DeadFeedback()
        {
            Vector2 middleOfGrid = new Vector2();
            middleOfGrid.X = myGridContainer.GetInitialWidth() / 2 * AbstractBlock.GetTileSize(); 
            middleOfGrid.Y = myGridContainer.GetInitialHeight() / 2 * AbstractBlock.GetTileSize();
            SpriteFeedback losar = new SpriteFeedback("losar", myOffset + middleOfGrid, () =>
            {
                return myIsDead == false;
            });
            FeedbackManager.AddFeedback(losar);
        }

        private void OnChainIncrement(int aChainLength, HashSet<AbstractBlock> aMatchedBlocks)
        {
            Console.WriteLine($"Chain {aChainLength}!");
            myChainTimer = Math.Max(1.0f, myChainTimer + 1.0f);

            AbstractBlock rightMostTopMostBlock = aMatchedBlocks.First();

            foreach(AbstractBlock block in aMatchedBlocks)
            {
                if (block.GetPosition().X > rightMostTopMostBlock.GetPosition().X)
                {
                    rightMostTopMostBlock = block;
                }
                else if (block.GetPosition().X == rightMostTopMostBlock.GetPosition().X)
                {
                    if (block.GetPosition().Y > rightMostTopMostBlock.GetPosition().Y)
                    {
                        rightMostTopMostBlock = block;
                    }
                }
            }

            Vector2 blockScreenPosition = rightMostTopMostBlock.GetScreenPosition(myOffset, myGridContainer.GetInitialHeight() - 1, myRaisingOffset);
            Vector2 feedbackOffset = new Vector2(AbstractBlock.GetTileSize(), AbstractBlock.GetTileSize()) / 2f;
            Vector2 feedbackPosition = blockScreenPosition + feedbackOffset;
            FeedbackManager.AddFeedback(new ChainComboFeedback("x", aChainLength, feedbackPosition));
        }

        private void OnChainEnd(int ChainLength)
        {
            GameInfo.GameInfo.SendMyRegards(new AngryInfo(new Point(myGridContainer.GetInitialWidth(), ChainLength -1), myPlayerIndex, EAngryType.Normal));
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
                    //Check for angryblocks above and below and to the sides, if found, tell it that it should be contaminated
                    //contaminted recusrivleky finds out all affected neighbor angrylocks.
                    //yes.
                    //while converting each angryblock to colorblock grid should be frozen/paused/watchemacallit
                    //
                    i++;
                }


                //find adjacent blocks to matchedblocks
                ContaminateBlocks(matchedBlocks);
            }

            if (matchedBlocks.Count > 3)
            {
                OnCombo(matchedBlocks);
            }

            if (hasChained)
            {
                myChainCounter++;
                OnChainIncrement(myChainCounter, matchedBlocks);
            }
        }

        private void ContaminateBlocks(HashSet<AbstractBlock> aMatchedBlocks)
        {
            HashSet<AbstractBlock> adjacentBlocks = myGridContainer.GetAdjacentBlocks(aMatchedBlocks);
            List<AbstractBlock> angryBlocks = new List<AbstractBlock>();
            foreach (AbstractBlock block in adjacentBlocks)
            {
                //mb block is null?
                if (block is AngryBlock angryBlock && angryBlock.IsBusy() == false)
                {
                    angryBlock.Contaminate(ref angryBlocks);
                }
            }
            foreach (var angryBundle in myAngryBundles)
            {
                if (angryBundle.IsContaminated())
                {
                    angryBlocks.Concat(angryBundle.GetBlocksForIteratingPurposes());
                }
            }
            angryBlocks = angryBlocks.OrderByDescending(block =>
            {
                return block.GetPosition().Y * -100 + block.GetPosition().X;
            }).ToList();

            const float MagicFrameAmount = 30f; //change to int, because of frames?
            myBlockIterators.Add(new BlockTimedIterator(AngryBlock.HandleBopStatic, angryBlocks, MagicFrameAmount, FinnishAngryBops));
            //create iterator
        }

        private void FinnishAngryBops(List<AbstractBlock> aPreviousBlocks)
        {
            foreach (var block in aPreviousBlocks)
            {
                AngryBlock angryBlock = block as AngryBlock;
                angryBlock.HandleDisintegrating();
            }
        }

        private void OnCombo(HashSet<AbstractBlock> aMatchedBlocks)
        {
            Console.WriteLine($"Combo! {aMatchedBlocks.Count}");
            const float MagicNumber = 0.3f;
            if (myChainTimer < 0.0f)
                myChainTimer = 0.0f;
            myChainTimer += (aMatchedBlocks.Count * MagicNumber) / myModifiedGameSpeed;
            CreateAngryComboBlock(aMatchedBlocks.Count -1);

            AbstractBlock rightMostTopMostBlock = aMatchedBlocks.First();

            foreach (AbstractBlock block in aMatchedBlocks)
            {
                if (block.GetPosition().X > rightMostTopMostBlock.GetPosition().X)
                {
                    rightMostTopMostBlock = block;
                }
                else if (block.GetPosition().X == rightMostTopMostBlock.GetPosition().X)
                {
                    if (block.GetPosition().Y > rightMostTopMostBlock.GetPosition().Y)
                    {
                        rightMostTopMostBlock = block;
                    }
                }
            }

            Vector2 blockScreenPosition = rightMostTopMostBlock.GetScreenPosition(myOffset, myGridContainer.GetInitialHeight() - 1, myRaisingOffset);
            Vector2 feedbackOffset = new Vector2(AbstractBlock.GetTileSize(), AbstractBlock.GetTileSize()) / 2f;
            Vector2 feedbackPosition = blockScreenPosition + feedbackOffset;
            FeedbackManager.AddFeedback(new ChainComboFeedback("", aMatchedBlocks.Count, feedbackPosition));
        }

        private void CreateAngryComboBlock(int aComboSize)
        {
            while(aComboSize > 2)
            {
                switch (aComboSize)
                {
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        CreateAngryCombo(aComboSize);
                        aComboSize -= aComboSize;
                        break;
                    case 7:
                        CreateAngryCombo(4);
                        CreateAngryCombo(3);
                        aComboSize -= 7;
                        break;
                    case 8:
                        CreateAngryCombo(4);
                        CreateAngryCombo(4);
                        aComboSize -= 8;
                        break;
                    case 9:
                        CreateAngryCombo(5);
                        CreateAngryCombo(4);
                        aComboSize -= 9;
                        break;

                    default:
                        CreateAngryCombo(6);
                        aComboSize -= 6;
                        break;
                }
            }
        }

        private void CreateAngryCombo(int aWidth)
        {
            GameInfo.GameInfo.SendMyRegards(new AngryInfo(new Point(aWidth, 1), myPlayerIndex, EAngryType.Normal));
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

        // +Player actions!
        public void SwapBlocksRight(Point aPosition)
        {
            AbstractBlock leftBlock = myGridContainer.GetBlockAtPosition(aPosition);
            AbstractBlock rightBlock = myGridContainer.GetBlockAtPosition(aPosition + new Point(1, 0));

            if ((leftBlock is ColorBlock || leftBlock is EmptyBlock) && leftBlock.IsSwitching() == false)
            {
                if ((rightBlock is ColorBlock || rightBlock is EmptyBlock) && rightBlock.IsSwitching() == false)
                {
                    leftBlock.SetPosition(aPosition + new Point(1, 0));
                    rightBlock.SetPosition(aPosition);

                    myGridContainer.myGrid[aPosition.Y][aPosition.X].SetBlock(rightBlock);
                    myGridContainer.myGrid[aPosition.Y][aPosition.X + 1].SetBlock(leftBlock);

                    float magicSwitchTime = 0.66f / myModifiedGameSpeed;
                    leftBlock.StartTheSwitchingCalculation(magicSwitchTime, ESwitchDirection.ToTheRight);
                    rightBlock.StartTheSwitchingCalculation(magicSwitchTime, ESwitchDirection.ToTheLeft);
                }
            }
        }

        public void AddAngryBundle(AngryBlockBundle aAngryBundle)
        {
            myAngryBundles.Add(aAngryBundle);
        }
        // -Player Actions

        public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach (AbstractBlock iBlock in myGridContainer.myBlocks)
            {
                iBlock.Draw(aSpriteBatch, myOffset, myGridContainer.GetInitialHeight() - 1, myRaisingOffset);
            }

            myBorderSprite.SetPosition(new Vector2(myOffset.X - 2, 6 - AbstractBlock.GetTileSize()));
            myBorderSprite.Draw(aSpriteBatch);

            aSpriteBatch.DrawString(myFont, 
                "Bonus time: " + myChainTimer.ToString() + 
                "\nGame Time: " + ((int)(myGameTime / 60)).ToString() + ":" + ((int)(myGameTime % 60)).ToString() +
                "\nGame Speed: " + myGameSpeed.ToString() +
                "\nLog Speed: " + myModifiedGameSpeed.ToString()
                , new Vector2(900, 100), Color.MidnightBlue);
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
                    return true;

                if (blocky is AngryBlock angry)
                {
                    if (angry.IsContaminated())
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
            RaiseAmount *= 0.1f;
            if (myWantsToRaiseBlocks && MyManualRaisingSpeed > RaiseAmount)
            {
                RaiseAmount = MyManualRaisingSpeed;
            }

            myRaisingOffset -= (RaiseAmount / AbstractBlock.GetTileSize());

            if (RaisingOffsetExceededTile() == true)
            {
                RearrangeRaisedTiles();
                myRaisingOffset = 0.0f;
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

        public bool RectangleIntersectsForFallingPurposes(Rectangle aRectangle)
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
    }
}
