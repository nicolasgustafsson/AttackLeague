using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Sprite
{
    class SpriteTilesetAngry : SpriteTileset
    {
        // Angry sprite tiles are structured like this
        //[_][_ _ _]
        //[ ][     ]
        //[ ][     ]
        //[_][_ _ _]

        public SpriteTilesetAngry()
            :base("angrytemplate")
        {
            myRectangle.Width = myTexture.Width / 4;
            myRectangle.Height = myTexture.Height / 4;
        }

        public void SetToAlone()
        {
            myRectangle.X = 0;
            myRectangle.Y = 0;
        }

        public void SetToTopLeft()
        {
            myRectangle.X = myRectangle.Width * 1;
            myRectangle.Y = myRectangle.Height * 1;
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, myRectangle, myColor, 0f, Vector2.Zero, myScale, SpriteEffects.None, 0);
        }
    }
}
