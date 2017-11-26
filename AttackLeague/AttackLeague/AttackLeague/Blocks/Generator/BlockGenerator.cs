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
        private GridContainer myGridContainer;

        //ColorBlock GenerateColorBlock();

        public BlockGenerator(GridContainer aGrid)
        {
            myGridContainer = aGrid;
        }

        public void GenerateGrid()
        {
            myGridContainer.OnGridReset();
            myGridContainer.GenerateTiles();

            myGridContainer.myBlocks = new List<AbstractBlock>();

            for (int columns = 0; columns < myGridContainer.GetWidth(); ++columns)
            {
                FrozenBlock block = new FrozenBlock();
                block.SetPosition(columns, 0);
                myGridContainer.myBlocks.Add(block);

                Tile tiley = myGridContainer.myGrid[0][columns];
                tiley.SetBlock(block);
            }

            for (int rows = 1; rows < myGridContainer.GetHeight(); ++rows)
            {
                for (int columns = 0; columns < myGridContainer.GetWidth(); ++columns)
                {
                    ColorBlock block = new ColorBlock();
                    block.SetPosition(columns, rows);
                    myGridContainer.myBlocks.Add(block);

                    Tile tiley = myGridContainer.myGrid[rows][columns];
                    tiley.SetBlock(block);
                }
            }

            foreach (AbstractBlock block in myGridContainer.myBlocks)
            {
                block.LoadContent();
            }

            myGridContainer.PrintGrid();
        }

        public FrozenBlock GenerateFrozenBlockAtPosition(Point aPosition)
        {
            FrozenBlock block = new FrozenBlock(GetRandomizedColor());
            block.LoadContent();

            myGridContainer.SetBlock(aPosition, block);

            return block;
        }

        public EmptyBlock GenerateEmptyBlockAtPosition(Point aPosition)
        {
            EmptyBlock block = new EmptyBlock();
            block.LoadContent();

            myGridContainer.SetBlock(aPosition, block);

            return block;
        }

        public EBlockColor GetRandomizedColor()
        {
            return (EBlockColor)Randomizer.GlobalRandomizer.Next(0, 5); // DO SUPER COOL RANDOMIZE
        }

        public void RemoveBlock(AbstractBlock aBlock, int aAmountOfDisappearingBlocks, int aCurrentBlock)
        {

            Point position = aBlock.GetPosition();

            int animationTime = 30;

            int delayBetweenAnimations = 15;

            int currentBlockDelta = aCurrentBlock * delayBetweenAnimations;

            int totalAnimationtime = aAmountOfDisappearingBlocks * delayBetweenAnimations + animationTime;

            int blockIndex = myGridContainer.myBlocks.LastIndexOf(aBlock);

            if (blockIndex == -1)
            {
                return;
            }
            aBlock = new DisappearingBlock(((AbstractColorBlock)aBlock).GetColor(),
                totalAnimationtime,
                currentBlockDelta);

            myGridContainer.InitializeBlock(position, aBlock);
        }

        private void MoveRowUp(int aRowNumber)
        {
            for (int columns = 0; columns < myGridContainer.GetWidth(); ++columns)
            {
                myGridContainer.myGrid[aRowNumber + 1][columns] = myGridContainer.myGrid[aRowNumber][columns];
                myGridContainer.myGrid[aRowNumber + 1][columns].GetBlock().SetPosition(columns, aRowNumber + 1);
            }
        }

        public void RearrangeRaisedTiles()
        {
            for (int rows = myGridContainer.myGrid.Count() - 1; rows >= 1; rows--)
            {
                if (myGridContainer.RowIsEmpty(rows) == false)
                {
                    if (myGridContainer.HasRow(rows + 1) == false)
                    {
                        AddEmptyRow();
                    }

                    MoveRowUp(rows);
                }
            }
            ConvertFrozenRowToColorBlocks();
            CreateFrozenRow();
        }

        public void AddEmptyRow()
        {
            myGridContainer.myGrid.Add(new List<Tile>());
            int row = myGridContainer.myGrid.Count() - 1;
            for (int columns = 0; columns < myGridContainer.GetWidth(); ++columns)
            {
                EmptyBlock block = new EmptyBlock();
                block.SetPosition(columns, row);
                myGridContainer.myBlocks.Add(block);

                Tile tiley = new Tile();
                tiley.SetBlock(block);
                myGridContainer.myGrid[row].Add(tiley);
            }
        }

        private void CreateFrozenRow()
        {
            for (int columns = 0; columns < myGridContainer.GetWidth(); ++columns)
            {
                AbstractBlock blocky = myGridContainer.myGrid[0][columns].GetBlock();
                Debug.Assert(blocky is FrozenBlock);
                HashSet<AbstractBlock> matchingBlocks = new HashSet<AbstractBlock>();
                do
                {
                    matchingBlocks.Clear();
                    ((FrozenBlock)(myGridContainer.myGrid[0][columns].GetBlock())).RandomizeColor();
                    CheckMatchesDirectionFrozenPurposes((AbstractColorBlock)blocky, new Point(0, 1), matchingBlocks);
                    CheckMatchesDirectionFrozenPurposes((AbstractColorBlock)blocky, new Point(-1, 0), matchingBlocks);
                } while (matchingBlocks.Any());
            }
        }

        private void ConvertFrozenRowToColorBlocks()
        {
            for (int column = 0; column < myGridContainer.GetWidth(); column++)
            {
                AbstractBlock blocky = myGridContainer.myGrid[0][column].GetBlock();
                Debug.Assert(blocky is FrozenBlock);
                ColorBlock colorBlocky = new ColorBlock(((FrozenBlock)blocky).GetColor());
                colorBlocky.SetPosition(new Point(column, 1));
                Tile tiley = new Tile();
                tiley.SetBlock(colorBlocky);

                myGridContainer.myGrid[1][column] = tiley;
                myGridContainer.myBlocks.Add(colorBlocky);
                colorBlocky.LoadContent();
            }
        }

        private void CheckMatchesDirectionFrozenPurposes(AbstractColorBlock aBlock, Point aOffset, HashSet<AbstractBlock> aMatchingBlocks)
        {
            Point blockPosition = aBlock.GetPosition();
            Point offsetPosition = blockPosition + aOffset;
            AbstractBlock directionBlock = myGridContainer.GetBlockAtPosition(offsetPosition.X, offsetPosition.Y);
            if (directionBlock is AbstractColorBlock && ((AbstractColorBlock)directionBlock).GetColor() == aBlock.GetColor())
            {
                Point offsetOffsetPosition = blockPosition + new Point(aOffset.X * 2, aOffset.Y * 2);
                AbstractBlock directionDirectionBlock = myGridContainer.GetBlockAtPosition(offsetOffsetPosition.X, offsetOffsetPosition.Y);
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
