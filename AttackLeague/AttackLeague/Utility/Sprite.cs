using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility
{
    public class Sprite
    {
        private Texture2D myTexture;
        private Vector2 myPosition;

        public Sprite(string aTextureName, ContentManager aContent)
        {
            myTexture = aContent.Load<Texture2D>(aTextureName);
        }

        public Vector2 GetSize()
        {
            return new Vector2(myTexture.Width, myTexture.Height);
        }

        public void SetPosition(Vector2 aNewPosition)
        {
            myPosition = aNewPosition;
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
