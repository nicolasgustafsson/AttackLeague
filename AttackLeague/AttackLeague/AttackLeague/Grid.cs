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

        public Grid()
        {
        }

        public void GenerateGrid(ContentManager aContent)
        {
            myGrid = new List<List<Tile>>();
            myBlocks = new List<AbstractBlock>();

            for (int rows = 0; rows < myHeight; ++rows)
            {
                myGrid.Add(new List<Tile>());
                for (int columns = 0; columns < myWidth; ++columns)
                {
                    myGrid[rows].Add(new Tile());

                    ColorBlock block = new ColorBlock();
                    block.SetPosition(columns, rows);
                    myBlocks.Add(block);
                }
            }

            foreach (AbstractBlock block in myBlocks)
            {
                block.LoadContent(aContent);
            }
        }

        public void LoadContent(ContentManager aContent)
        {
            GenerateGrid(aContent);

        }

        public void Update()
        {
            myBlocks.Sort();

            for (int i = myBlocks.Count - 1; i >= 0; --i)
            {
                AbstractBlock block = myBlocks[i];
                block.Update();
            }

            CheckForMatches();
        }

        private void CheckForMatches()
        {
            HashSet<int> matchedBlocks = new HashSet<int>();
            for (int i = myBlocks.Count - 1; i >= 0; --i)
            {
                AbstractBlock block = myBlocks[i];

                matchedBlocks.UnionWith(CheckMatches(block));
            }

            if (matchedBlocks.Count > 0 )
            {
                foreach (int index in matchedBlocks)
                {
                    Console.WriteLine(myBlocks[index].GetPosition());
                    RemoveBlock(index);
                }
            }
        }

        private HashSet<int> CheckMatches(AbstractBlock aBlock)
        {
            HashSet<int> matchingIndices = new HashSet<int>();
            if (aBlock is ColorBlock )
            {
                ColorBlock block = (ColorBlock)aBlock;
                BlockColor currentColor = block.GetColor();


                CheckMatchHorizontal(matchingIndices, block, currentColor);

                CheckMatchVertical(matchingIndices, block, currentColor);

            }
            return matchingIndices;
        }

        /// <summary>
        /// Checks for matches to the ups
        /// </summary>
        private void CheckMatchVertical(HashSet<int> aMatchingIndices, ColorBlock aBlock, BlockColor aBlockColor)
        {
            Point blockPosition = aBlock.GetPosition();
            if (blockPosition.Y > 1)
            {
                AbstractBlock upBlock = myBlocks[GetIndexFromPosition(blockPosition.X, blockPosition.Y - 1)];
                if (upBlock is ColorBlock && ((ColorBlock)upBlock).GetColor() == aBlockColor)
                {
                    AbstractBlock upUpBlock = myBlocks[GetIndexFromPosition(blockPosition.X, blockPosition.Y - 2)];
                    if (upUpBlock is ColorBlock && ((ColorBlock)upUpBlock).GetColor() == aBlockColor)
                    {
                        aMatchingIndices.Add(GetIndexFromBlock(aBlock));
                        aMatchingIndices.Add(GetIndexFromBlock(upBlock));
                        aMatchingIndices.Add(GetIndexFromBlock(upUpBlock));
                    }
                }
            }
        }

        /// <summary>
        /// Checks for matches to the left
        /// </summary>
        private void CheckMatchHorizontal(HashSet<int> aMatchingIndices, ColorBlock aBlock, BlockColor aBlockColor)
        {
            Point blockPosition = aBlock.GetPosition();
            if (blockPosition.X > 1)
            {
                AbstractBlock leftBlock = myBlocks[GetIndexFromPosition(blockPosition.X - 1, blockPosition.Y)];
                if (leftBlock is ColorBlock && ((ColorBlock)leftBlock).GetColor() == aBlockColor)
                {
                    AbstractBlock leftLeftBlock =
                        myBlocks[GetIndexFromPosition(blockPosition.X - 2, blockPosition.Y)];
                    if (leftLeftBlock is ColorBlock && ((ColorBlock)leftLeftBlock).GetColor() == aBlockColor)
                    {
                        aMatchingIndices.Add(GetIndexFromBlock(aBlock));
                        aMatchingIndices.Add(GetIndexFromBlock(leftBlock));
                        aMatchingIndices.Add(GetIndexFromBlock(leftLeftBlock));
                    }
                }
            }
        }

        private int GetIndexFromBlock(AbstractBlock aBlock)
        {
            return GetIndexFromPosition(aBlock.GetPosition());
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

            return myBlocks[aX * aY];
        }


        private int GetIndexFromPosition(Point aPosition)
        {
            return GetIndexFromPosition(aPosition.X, aPosition.Y);
        }

        private int GetIndexFromPosition(int aX, int aY)
        {
            return aX + aY * myWidth;
        }

        private void RemoveBlock(int aIndex)
        {
            if (aIndex >= myBlocks.Count)
            {
                Console.WriteLine("Removal???");
                return;
            }

            Point position = myBlocks[aIndex].GetPosition();

            myBlocks[aIndex] = new EmptyBlock();

            myBlocks[aIndex].SetPosition(position.X, position.Y);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach (AbstractBlock iBlock in myBlocks)
            {
                iBlock.Draw(aSpriteBatch, new Vector2(100, 100));
            }
        }
    }
}
