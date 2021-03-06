﻿using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.AttackLeague.Grid;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks.Generator
{
    // illuminati confirm
    // contains all utility and thingies to create blocks properly
    public class BlockGenerator
    {
        private List<EBlockColor> myAllowedColors;
        public GridBundle myGridBundle; //REMOAVE DIS
        private int myNextAngryIndex = 0;

        //ColorBlock GenerateColorBlock();

        public BlockGenerator()
        {
        }

        public void SetBundle(GridBundle aBundle)
        {
            myGridBundle = aBundle;
        }

        public void GenerateGrid()
        {
            if (myGridBundle.Behavior != null)
            {
                myGridBundle.Behavior.OnGridReset();
            }
            myGridBundle.Container.RemoveAll();
            myGridBundle.Container.GenerateTiles();

            myGridBundle.Container.myBlocks = new List<AbstractBlock>();

            for (int columns = 0; columns < myGridBundle.Container.GetInitialWidth(); ++columns)
            {
                FrozenBlock block = new FrozenBlock(myGridBundle);
                block.SetPosition(columns, 0);
                myGridBundle.Container.myBlocks.Add(block);

                Tile tiley = myGridBundle.Container.myGrid[0][columns];
                tiley.SetBlock(block);
            }

            for (int rows = 1; rows < myGridBundle.Container.GetInitialHeight(); ++rows)
            {
                for (int columns = 0; columns < myGridBundle.Container.GetInitialWidth(); ++columns)
                {
                    EmptyBlock block = new EmptyBlock(myGridBundle);
                    //ColorBlock block = new ColorBlock(myGridBundle);
                    block.SetPosition(columns, rows);
                    myGridBundle.Container.myBlocks.Add(block);

                    Tile tiley = myGridBundle.Container.myGrid[rows][columns];
                    tiley.SetBlock(block);
                }
            }

            foreach (AbstractBlock block in myGridBundle.Container.myBlocks)
            {
                block.LoadContent();
            }

            myGridBundle.Container.PrintGrid();
            myGridBundle.Container.EnsureUnique();

        }

        public FrozenBlock GenerateFrozenBlockAtPosition(Point aPosition)
        {
            FrozenBlock block = new FrozenBlock(myGridBundle, GetRandomizedColor());
            block.LoadContent();

            myGridBundle.Container.SetBlock(aPosition, block);

            return block;
        }

        public void EliminateBlock(Point aPosition)
        {
            myGridBundle.Container.InitializeBlock(aPosition, new EmptyBlock(myGridBundle));
        }


        public EmptyBlock GenerateEmptyBlockAtPosition(Point aPosition)
        {
            EmptyBlock block = new EmptyBlock(myGridBundle);
            block.LoadContent();

            myGridBundle.Container.SetBlock(aPosition, block);

            return block;
        }

        public void ExpandToHeight(int aHeight)
        {
            int heightBeforePushingTheLimits = myGridBundle.Container.GetCurrentHeight();

            for (int i = heightBeforePushingTheLimits; i < aHeight; i++)
                AddEmptyRow();
        }

        public AngryBlockBundle CreateAngryBlockBundleAtPosition(Point aPosition, Point aSize)
        {
            return CreateAngryBlockBundleAtPosition(aPosition, aSize.X, aSize.Y);
        }

        public AngryBlockBundle CreateAngryBlockBundleAtPosition(Point aPosition, int aWidth, int aHeight) //TETRIS!!
        {
            AngryBlockBundle angryBundle = new AngryBlockBundle(myGridBundle);

            if (aPosition.Y + aHeight > myGridBundle.Container.GetInitialHeight())
            {
                ExpandToHeight(aPosition.Y + aHeight);
            }

            for (int y = 0; y < aHeight; y++)
            {
                for (int x = 0; x < aWidth; x++)
                {
                    AngryBlock tempy = new AngryBlock(myGridBundle, y + 1, angryBundle);
                    tempy.LoadContent();
                    myGridBundle.Container.SetBlock(new Point(aPosition.X + x, aPosition.Y + y), tempy);
                }
            }
            angryBundle.Initialize(myNextAngryIndex++);
            return angryBundle;
        }

        public EBlockColor GetRandomizedColor()
        {
            return (EBlockColor)myGridBundle.GridRandomizer.Next(0, 5); // DO SUPER COOL RANDOMIZE
        }

        public void RemoveBlock(AbstractBlock aBlock, int aAmountOfDisappearingBlocks, int aCurrentBlock)
        {

            Point position = aBlock.GetPosition();

            int animationTime = 30;

            int delayBetweenAnimations = 15;

            int currentBlockDelta = aCurrentBlock * delayBetweenAnimations;

            int totalAnimationtime = aAmountOfDisappearingBlocks * delayBetweenAnimations + animationTime;

            int blockIndex = myGridBundle.Container.myBlocks.LastIndexOf(aBlock);

            if (blockIndex == -1)
            {
                return;
            }
            aBlock = new DisappearingBlock(myGridBundle, ((AbstractColorBlock)aBlock).GetColor(),
                totalAnimationtime,
                currentBlockDelta);

            myGridBundle.Container.InitializeBlock(position, aBlock);
        }

        private void MoveRowUp(int aRowNumber)
        {
            for (int columns = 0; columns < myGridBundle.Container.GetInitialWidth(); ++columns)
            {
                myGridBundle.Container.myGrid[aRowNumber + 1][columns] = myGridBundle.Container.myGrid[aRowNumber][columns];
                myGridBundle.Container.myGrid[aRowNumber + 1][columns].GetBlock().SetPosition(columns, aRowNumber + 1); //here
            }
        }


        public void RearrangeRaisedTiles()
        {
            int topRow = myGridBundle.Container.GetCurrentHeight() -1;

            bool topRowIsEmpty = true;
            for (int x = 0; x < myGridBundle.Container.GetInitialWidth(); x++)
            {
                if (myGridBundle.Container.myGrid[topRow][x].GetBlock() is EmptyBlock == false)
                {
                    topRowIsEmpty = false;
                }
            }
            if (topRowIsEmpty == false)
            {
                AddEmptyRow();
            }

            int rowBeneathTopRow = myGridBundle.Container.GetCurrentHeight() - 2;
            for (int rows = rowBeneathTopRow; rows >= 1; rows--)
            {
                MoveRowUp(rows);

                //if (myGridBundle.Container.RowIsEmpty(rows) == false)
                //{
                //}
            }
            ConvertFrozenRowToColorBlocks();
            CreateFrozenRow();
            myGridBundle.Container.EnsureUnique();
        }

        public void AddEmptyRow()
        {
            myGridBundle.Container.myGrid.Add(new List<Tile>());
            int row = myGridBundle.Container.myGrid.Count() - 1;
            for (int columns = 0; columns < myGridBundle.Container.GetInitialWidth(); ++columns)
            {
                Tile tiley = new Tile();
                myGridBundle.Container.myGrid[row].Add(tiley);

                EmptyBlock block = new EmptyBlock(myGridBundle);
                block.SetPosition(columns, row); //here
                myGridBundle.Container.myBlocks.Add(block);
                tiley.SetBlock(block);

                //myGridBundle.Container.SetBlock(new Point(columns, row), block);

            }
            //myGridBundle.Container.EnsureUnique();

        }

        private void CreateFrozenRow()
        {
            for (int columns = 0; columns < myGridBundle.Container.GetInitialWidth(); ++columns)
            {
                AbstractBlock blocky = myGridBundle.Container.myGrid[0][columns].GetBlock();
                Debug.Assert(blocky is FrozenBlock);
                HashSet<AbstractBlock> matchingBlocks = new HashSet<AbstractBlock>();
                do
                {
                    matchingBlocks.Clear();
                    ((FrozenBlock)(myGridBundle.Container.myGrid[0][columns].GetBlock())).RandomizeColor();
                    CheckMatchesDirectionFrozenPurposes((AbstractColorBlock)blocky, new Point(0, 1), matchingBlocks);
                    CheckMatchesDirectionFrozenPurposes((AbstractColorBlock)blocky, new Point(-1, 0), matchingBlocks);
                } while (matchingBlocks.Any());
            }
        }

        private void ConvertFrozenRowToColorBlocks()
        {
            for (int column = 0; column < myGridBundle.Container.GetInitialWidth(); column++)
            {
                AbstractBlock blocky = myGridBundle.Container.myGrid[0][column].GetBlock();
                Debug.Assert(blocky is FrozenBlock);
                ColorBlock colorBlocky = new ColorBlock(myGridBundle, ((FrozenBlock)blocky).GetColor());
                colorBlocky.SetPosition(new Point(column, 1));
                Tile tiley = new Tile();
                tiley.SetBlock(colorBlocky);

                myGridBundle.Container.myGrid[1][column] = tiley;
                myGridBundle.Container.myBlocks.Add(colorBlocky);
                colorBlocky.LoadContent();
            }
        }

        public void ConvertFrozenBlockToFallingOrColorBlock(FrozenBlock aBlock)
        {
            AbstractBlock blockBeneath = myGridBundle.Container.GetBlockAtPosition(aBlock.GetPosition() + new Point(0, -1));
            AbstractBlock newBlock;
            if (blockBeneath.AllowsFalling())
            {
                newBlock = new FallingBlock(myGridBundle, aBlock.GetColor());
                ((FallingBlock)newBlock).CanChain = true;
            }
            else
                newBlock = new ColorBlock(myGridBundle, aBlock.GetColor(), true);

            myGridBundle.Container.SetBlock(aBlock.GetPosition(), newBlock);
            newBlock.LoadContent();
        }

        private void CheckMatchesDirectionFrozenPurposes(AbstractColorBlock aBlock, Point aOffset, HashSet<AbstractBlock> aMatchingBlocks)
        {
            Point blockPosition = aBlock.GetPosition();
            Point offsetPosition = blockPosition + aOffset;
            AbstractBlock directionBlock = myGridBundle.Container.GetBlockAtPosition(offsetPosition.X, offsetPosition.Y);
            if (directionBlock is AbstractColorBlock && ((AbstractColorBlock)directionBlock).GetColor() == aBlock.GetColor())
            {
                Point offsetOffsetPosition = blockPosition + new Point(aOffset.X * 2, aOffset.Y * 2);
                AbstractBlock directionDirectionBlock = myGridBundle.Container.GetBlockAtPosition(offsetOffsetPosition.X, offsetOffsetPosition.Y);
                if (directionDirectionBlock is AbstractColorBlock && ((AbstractColorBlock)directionDirectionBlock).GetColor() == aBlock.GetColor())
                {
                    aMatchingBlocks.Add(aBlock);
                    aMatchingBlocks.Add(directionBlock);
                    aMatchingBlocks.Add(directionDirectionBlock);
                }
            }
        }
    }
}
