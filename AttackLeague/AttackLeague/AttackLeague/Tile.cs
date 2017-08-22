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
        private Vector2 myPosition;
        
        void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset)
        {
            if (myBlock != null)
            {
                myBlock.Draw(aSpriteBatch, aGridOffset, myPosition);
            }
        }

    }
}
