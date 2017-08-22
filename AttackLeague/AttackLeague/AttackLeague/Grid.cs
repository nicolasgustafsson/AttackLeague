using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            for ( int rows = 0; rows < myHeight; ++rows)
            {
                myGrid.Add(new List<Tile>());
                for (int columns = 0; columns < myWidth; ++columns)
                {
                    myGrid[rows].Add(new Tile());
                }
            }
        }

        public void Update()
        {
            foreach (AbstractBlock iBlock in myBlocks)
            {

            }
        }


    }
}
