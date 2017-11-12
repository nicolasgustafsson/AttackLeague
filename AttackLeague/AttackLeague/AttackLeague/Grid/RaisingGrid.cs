using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Grid
{
    public class RaisingGrid : BaseGrid
    {
        //Super ugly pls fix
        private bool myHasRaisedThisFrame = false;
        protected bool myWantsToRaiseBlocks = false;
        protected float myRaisingOffset = 0f;
        private const float MyConstantRaisingSpeed = 3;

        public override void Update()
        {
            base.Update();
            myHasRaisedThisFrame = false;
        }

        private void RearrangeRaisedTiles()
        {
            for (int rows = myGrid.Count() - 1; rows >= 1; rows--)
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
                HashSet<AbstractBlock> matchingBlocks = new HashSet<AbstractBlock>();
                do
                {
                    matchingBlocks.Clear();
                    ((FrozenBlock)(myGrid[0][columns].GetBlock())).RandomizeColor();
                    CheckMatchesDirectionFrozenPurposes((AbstractColorBlock)blocky, new Point(0, 1), matchingBlocks);
                    CheckMatchesDirectionFrozenPurposes((AbstractColorBlock)blocky, new Point(-1, 0), matchingBlocks);
                } while (matchingBlocks.Any());
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
                colorBlocky.LoadContent();

            }
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

        protected void MoveRowUp(int aRowNumber)
        {
            for (int columns = 0; columns < myWidth; ++columns)
            {
                myGrid[aRowNumber + 1][columns] = myGrid[aRowNumber][columns];
                myGrid[aRowNumber + 1][columns].GetBlock().SetPosition(columns, aRowNumber + 1);
            }
        }

        protected bool RaisingOffsetExceededTile()
        {
            return myRaisingOffset < -1f;
        }

        public bool HasRaisedGridThisFrame()
        {
            return myHasRaisedThisFrame;
        }

        protected bool IsExceedingRoof()
        {
            for (int column = 0; column < myWidth; column++)
            {
                if (ColumnIsExceedingRoof(column) == true)
                    return true;
            }

            return false;
        }

        public bool ColumnIsExceedingRoof(int aColumn)
        {
            if (myGrid.Count() <= myHeight)
                return false;
            if (myGrid[myHeight][aColumn].GetBlock() is EmptyBlock == false)
                return true;
            return false;
        }

        //           ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //            | | | | | | | | | | NO ENTRY | | | | | | | | |
        private void CheckMatchesDirectionFrozenPurposes(AbstractColorBlock aBlock, Point aOffset, HashSet<AbstractBlock> aMatchingBlocks)
        {
            Point blockPosition = aBlock.GetPosition();
            Point offsetPosition = blockPosition + aOffset;
            AbstractBlock directionBlock = GetBlockAtPosition(offsetPosition.X, offsetPosition.Y);
            if (directionBlock is AbstractColorBlock && ((AbstractColorBlock)directionBlock).GetColor() == aBlock.GetColor())
            {
                Point offsetOffsetPosition = blockPosition + new Point(aOffset.X * 2, aOffset.Y * 2);
                AbstractBlock directionDirectionBlock = GetBlockAtPosition(offsetOffsetPosition.X, offsetOffsetPosition.Y);
                if (directionDirectionBlock is AbstractColorBlock && ((AbstractColorBlock)directionDirectionBlock).GetColor() == aBlock.GetColor())
                {
                    aMatchingBlocks.Add(aBlock);
                    aMatchingBlocks.Add(directionBlock);
                    aMatchingBlocks.Add(directionDirectionBlock);
                }
            }
        }
        //           ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //            | | | | | | | | | | NO ENTRY | | | | | | | | |
    }
}

