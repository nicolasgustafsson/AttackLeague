using AttackLeague.AttackLeague.Grid;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Blocks.Factory
{
    // illuminati confirm
    class BlockFactory
    {
        private List<EBlockColor> myAllowedColors;
        private GridContainer myGridContainer;

        //ColorBlock GenerateColorBlock();


        public void GenerateGrid()
        {
            for (int x = 0; x < myGridContainer.GetWidth(); x++)
            {
                for (int y = 0; y < myGridContainer.GetWidth(); y++)
                {
                    GenerateFrozenBlockAtPosition(new Point(x, y));
                }
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
    }
}
