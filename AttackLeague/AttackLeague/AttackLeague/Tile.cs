﻿using System;
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

        public bool IsEmpty()
        {
            return myBlock is EmptyBlock;
        }

        public bool CanFallThrough()
        {
            return myBlock.AllowsFalling() && myBlock.IsSwitching() == false; //(myBlock is EmptyBlock || myBlock is FallingBlock) && myBlock.IsSwitching() == false;
        }
    }
}
