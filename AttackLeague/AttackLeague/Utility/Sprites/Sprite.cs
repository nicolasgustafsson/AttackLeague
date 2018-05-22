using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;

namespace AttackLeague.Utility.Sprites
{
    [Serializable] 
    public class Sprite
    {
        public string myTextureName;
        public Color myColor;
        public Vector2 myPosition;

        protected Texture2D myTexture;
        protected Vector2 myScale = Vector2.One;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            myTexture = ContentManagerInstance.Content.Load<Texture2D>(myTextureName);
        }

        public Sprite()
        {

        }

        public Sprite(string aTextureName)
        {
            myColor = Color.White;
            myTexture = ContentManagerInstance.Content.Load<Texture2D>(aTextureName);
            myTextureName = myTexture.Name;
        }

        public Vector2 GetPosition()
        {
            return myPosition;
        }

        public virtual Vector2 GetSize()
        {
            return new Vector2(myTexture.Width, myTexture.Height);
        }

        public virtual Vector2 GetScaledSize()
        {
            return new Vector2(myTexture.Width * myScale.X, myTexture.Height * myScale.Y);
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
