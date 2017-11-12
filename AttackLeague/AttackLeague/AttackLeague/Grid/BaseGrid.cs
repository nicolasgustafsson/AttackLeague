using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace AttackLeague.AttackLeague.Grid
{
    public class BaseGrid
    {
        protected List<List<Tile>> myGrid;
        protected List<AbstractBlock> myBlocks;
        protected const int myHeight = 12;
        protected const int myWidth = 6;

        protected virtual void OnGridReset() { }

        public int GetHeight()
        {
            return myHeight;
        }

        public int GetWidth()
        {
            return myWidth;
        }

        public void GenerateGrid()
        {
            OnGridReset();
            
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
                block.LoadContent();
            }

            PrintGrid();
        }

        public virtual void Update()
        {

        }

        protected void PrintGrid()
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
        protected void CreateBlock(Point aPosition, AbstractBlock aBlock)
        {
            aBlock.SetPosition(aPosition);
            aBlock.LoadContent();

            AbstractBlock previousBlock = myGrid[aPosition.Y][aPosition.X].GetBlock();

            myBlocks[myBlocks.LastIndexOf(previousBlock)] = aBlock;

            myGrid[aPosition.Y][aPosition.X].SetBlock(aBlock);
        }

        protected AbstractBlock GetBlockAtPosition(Point aPosition)
        {
            return GetBlockAtPosition(aPosition.X, aPosition.Y);
        }

        protected AbstractBlock GetBlockAtPosition(int aX, int aY)
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

        protected void EliminateBlock(int aBlockIndex)
        {
            Point position = myBlocks[aBlockIndex].GetPosition();

            CreateBlock(position, new EmptyBlock());
        }

        protected void SetBlock(Point aPosition, AbstractBlock aBlock)
        {
            aBlock.SetPosition(aPosition);

            AbstractBlock previousBlock = myGrid[aPosition.Y][aPosition.X].GetBlock();

            myBlocks[myBlocks.LastIndexOf(previousBlock)] = aBlock;

            myGrid[aPosition.Y][aPosition.X].SetBlock(aBlock);
        }

        protected void RemoveBlock(AbstractBlock aBlock, int aAmountOfDisappearingBlocks, int aCurrentBlock)
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

        protected void AddEmptyRow()
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

        protected bool RowIsEmpty(int aRowNumber)
        {
            for (int columns = 0; columns < myWidth; columns++)
            {
                if ((myGrid[aRowNumber][columns].GetBlock() is EmptyBlock) == false)
                    return false;
            }
            return true;
        }

        protected bool HasRow(int aRowNumber)
        {
            return myGrid.Count() - 1 > aRowNumber;
        }

    }
}
