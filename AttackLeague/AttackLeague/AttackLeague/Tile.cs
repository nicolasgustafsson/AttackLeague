using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackLeague.AttackLeague
{
    public class Tile
    {
        private AbstractBlock myBlock;

        public void SetBlock(AbstractBlock aBlock)
        {
            myBlock = aBlock;
        }

        public AbstractBlock GetBlock()
        {
            return myBlock;
        }

    }
}
