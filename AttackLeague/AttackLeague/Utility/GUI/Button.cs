using System;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackLeague.Utility.GUI
{
    public delegate bool ButtonAction();

    class Button
    {
        protected Sprite mySprite;
        private Rectangle myHotspot;
        public ButtonAction OnClicked;    
        public ButtonAction OnLostFocus;

        //this is ylfs domain

        public Button()
        {
            MouseUtility.LeftPressedCallback += OnClickedFunction;
            // todo remove at destruction! Otherwise no garbagecollectingy!!
        }

        public virtual void SetSprite(string aTextureName, Point aPosition) // todo default args pls
        {
            mySprite = new Sprite(aTextureName);
            mySprite.SetPosition(new Vector2(aPosition.X, aPosition.Y));
            myHotspot = new Rectangle(aPosition.X, aPosition.Y, (int)mySprite.GetSize().X, (int)mySprite.GetSize().Y);
        }

        public virtual void SetSprite(string aTextureName, Point aPosition, Point aSize, bool aSetScale) // todo default args pls
        {
            SetSprite(aTextureName, aPosition);
            myHotspot.Width = aSize.X;
            myHotspot.Height = aSize.Y;
            if (aSetScale)
            {
                mySprite.SetScale(new Vector2(aSize.X, aSize.Y));
            }
        }

        public void SetSpriteColor(Color aColor)
        {
            mySprite.SetColor(aColor);
        }

        private void OnClickedFunction(object sender, EventArgs args)
        { 
            // todo only call if we are active GUI! handle active gui
            Point mousePos = Mouse.GetState().Position;
            if (myHotspot.Bottom > mousePos.Y &&
                myHotspot.Top<mousePos.Y &&
                myHotspot.Left<mousePos.X &&
                myHotspot.Right> mousePos.X)
            {
                OnClicked?.Invoke();
            }
            else
            {
                OnLostFocus?.Invoke();
            }
        }

        public virtual void Update()
        {
            //Point mousePos = Mouse.GetState().Position;
            //if (myHotspot.Bottom > mousePos.Y &&
            //    myHotspot.Top < mousePos.Y &&
            //    myHotspot.Left < mousePos.X &&
            //    myHotspot.Right > mousePos.X)
            //{
            //    OnHover();
            // todo bool on hover state and then we have enter hover and exit hover and such.
            //}
        }

        public virtual void OnHover()
        {
        }

        public virtual void Draw(SpriteBatch aSpriteBatch)
        {
            mySprite.Draw(aSpriteBatch);
        }
    }
}
