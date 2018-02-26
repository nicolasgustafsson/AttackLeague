using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace AttackLeague.Utility.GUI
{
    public delegate bool ButtonAction();

    class Button
    {
        public Sprite mySprite;
        public Rectangle myHotspot;
        public ButtonAction OnClicked;    

        //this is ylfs domain

        public Button()
        {
            MouseUtility.LeftPressedCallback += OnClickedFunction;
            // todo remove at destruction! Otherwise no garbagecollectingy!!
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
        }

        void Update()
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
    }
}
