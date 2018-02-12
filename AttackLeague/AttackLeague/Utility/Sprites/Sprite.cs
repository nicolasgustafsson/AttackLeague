using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AttackLeague.Utility.Sprites
{
    public class Sprite
    {
        protected Texture2D myTexture;
        protected Vector2 myPosition;
        protected Vector2 myScale = Vector2.One;
        protected Color myColor;

        public Sprite(string aTextureName)
        {
            myColor = Color.White;
            myTexture = ContentManagerInstance.Content.Load<Texture2D>(aTextureName);
        }

        public Vector2 GetPosition()
        {
            return myPosition;
        }

        public virtual Vector2 GetSize()
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

        public virtual void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, null, myColor, 0f, Vector2.Zero, myScale, SpriteEffects.None, 0);
        }
    }
}
