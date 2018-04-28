using System;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.IO;

namespace AttackLeague.Utility.GUI
{
    public delegate bool ButtonAction();

    [Serializable]
    class Button
    {
        public String myName = "Hello";

        public Sprite mySprite;

        protected Rectangle myHotspot;

        [NonSerialized]
        public ButtonAction OnClicked;    
        [NonSerialized]
        public ButtonAction OnLostFocus;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            SetHotspotBasedOnSprite();
        }
        //this is ylfs domain

        public Button()
        {
            MouseUtility.LeftPressedCallback += OnClickedFunction;
        }

        public virtual void SetSprite(string aTextureName, Point aPosition) // todo default args pls
        {
            mySprite = new Sprite(aTextureName);
            mySprite.SetPosition(new Vector2(aPosition.X, aPosition.Y));
            SetHotspotBasedOnSprite();
        }

        private void SetHotspotBasedOnSprite()
        {
            var Position = mySprite.GetPosition();
            myHotspot = new Rectangle((int)Position.X, (int)Position.Y, (int)mySprite.GetSize().X, (int)mySprite.GetSize().Y);
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

/*
 baseGUI
    parent
    children
    pos - abs / rel
    size - abs / rel
    alignment - right / left  up / down  center
    pivot

    Image
    Column
    Button
    Text
    Textbox
    Dropdown
    Slider
    Radiobuttons
    Checkbox -> Cool Checkbox w particles and everything
*/