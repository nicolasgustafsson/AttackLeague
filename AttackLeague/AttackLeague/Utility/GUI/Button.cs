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

        public Button()
        {
            //MouseUtility.LeftPressedCallback += OnClicked;
            // todo remove at destruction! Otherwise no garbagecollectingy!!
        }

        void Update()
        {
            Point mousePos = Mouse.GetState().Position;
            if (myHotspot.Bottom > mousePos.Y &&
                myHotspot.Top < mousePos.Y &&
                myHotspot.Left < mousePos.X &&
                myHotspot.Right > mousePos.X)
            {
                OnHover();
            }
        }

        public virtual void OnHover()
        {
        }
    }
}
