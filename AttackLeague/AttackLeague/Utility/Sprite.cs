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
        private Vector2 myScale = Vector2.One;
        private Color myColor;

        public Sprite(string aTextureName)
        {
            myColor = Color.White;
            myTexture = ContentManagerInstance.Content.Load<Texture2D>(aTextureName);
        }

        public Vector2 GetSize()
        {
            return new Vector2(myTexture.Width, myTexture.Height);
        }

        public void SetScale(Vector2 aNewScale)
        {
            myScale = aNewScale;
        }
        public void SetColor(Color aColor)
        {
            myColor = aColor;
        }

        public void SetPosition(Vector2 aNewPosition)
        {
            myPosition = aNewPosition;
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, null, myColor, 0f, Vector2.Zero, myScale, SpriteEffects.None, 0);
        }
    }
}
