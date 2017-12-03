using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Sprite
{
    public class SpriteTileset : Sprite
    {
        protected Rectangle myRectangle;

        public SpriteTileset(string aTexture)
        : base(aTexture)
        {
            myRectangle = myTexture.Bounds;
        }

        public void SetRectanglePixels(Rectangle aRectangle)
        {
            myRectangle = aRectangle;
        }

        public void SetRectangleNormalized(Rectangle aRectangle)
        {
            Rectangle size = myTexture.Bounds;

            myRectangle = new Rectangle(
                size.Width * aRectangle.X,
                size.Height * aRectangle.Y, 
                size.Width * aRectangle.Width, 
                size.Height * aRectangle.Height);
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, myRectangle, myColor, 0f, Vector2.Zero, myScale, SpriteEffects.None, 0);
        }

        public override Vector2 GetSize()
        {
            return new Vector2(myRectangle.Width, myRectangle.Height);
        }
    }
}
