using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using AttackLeague.AttackLeague.Blocks.Generator;
using System.Diagnostics;

namespace AttackLeague.AttackLeague.Grid
{
    public class GridContainer
    {
        public List<List<Tile>> myGrid;
        public List<AbstractBlock> myBlocks;
        private const int myHeight = 12;
        private const int myWidth = 6;
        private BlockGenerator myBlockGenerator;

        public virtual void OnGridReset() { }

        public GridContainer()
        {
        }

        public void SetGenerator(BlockGenerator aBlockGenerator)
        {
            myBlockGenerator = aBlockGenerator;
        }

        public BlockGenerator GetBlockGenerator()
        {
            Debug.Assert(myBlockGenerator != null);
            return myBlockGenerator;
        }

        public int GetHeight()
        {
            return myHeight;
        }

        public int GetWidth()
        {
            return myWidth;
        }

        //public void DebugRerandomizeGrid()
        //{
        //    myBlockGenerator.GenerateGrid();
        //}

        public void GenerateTiles()
        {
            myGrid = new List<List<Tile>>();

            for (int rows = 0; rows < myHeight; ++rows)
            {
                myGrid.Add(new List<Tile>());
                for (int columns = 0; columns < myWidth; ++columns)
                {
                    Tile tiley = new Tile();
                    myGrid[rows].Add(tiley);
                }
            }
        }

        public virtual void Update()
        {

        }

        public void PrintGrid()
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

        //fixes everything
        public void InitializeBlock(Point aPosition, AbstractBlock aBlock)
        {
            aBlock.LoadContent();
            SetBlock(aPosition, aBlock);
        }

        public AbstractBlock GetBlockAtPosition(Point aPosition)
        {
            return GetBlockAtPosition(aPosition.X, aPosition.Y);
        }

        public AbstractBlock GetBlockAtPosition(int aX, int aY)
        {
            if (aX < 0 || aX >= myWidth || aY < 0 || aY >= myGrid.Count)
            {
                return null;
            }

            return myGrid[aY][aX].GetBlock();
        }

        protected bool RectangleIntersects(Rectangle aRectangle)
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

        public void SetBlock(Point aPosition, AbstractBlock aBlock)
        {
            aBlock.SetPosition(aPosition);

            AbstractBlock previousBlock = myGrid[aPosition.Y][aPosition.X].GetBlock();

            myBlocks[myBlocks.LastIndexOf(previousBlock)] = aBlock;

            myGrid[aPosition.Y][aPosition.X].SetBlock(aBlock);
        }

        public bool RowIsEmpty(int aRowNumber)
        {
            for (int columns = 0; columns < myWidth; columns++)
            {
                if ((myGrid[aRowNumber][columns].GetBlock() is EmptyBlock) == false)
                    return false;
            }
            return true;
        }

        public bool HasRow(int aRowNumber)
        {
            return myGrid.Count() - 1 > aRowNumber;
        }

        public bool IsExceedingRoof()
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

        public bool ColumnIsCloseToExceedingRoof(int aColumn)
        {
            if (myGrid.Count() <= myHeight - 2)
                return false;
            if (myGrid[myHeight - 2][aColumn].GetBlock() is EmptyBlock == false)
                return true;
            return false;
        }
    }
}
