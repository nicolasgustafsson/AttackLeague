using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.GUI
{
    static class MouseUtility
    {
        // DO ALL THE EVENTS AND CALLBACKS AND WRAPPINGS

        public static event EventHandler LeftPressedCallback;
        public static event EventHandler LeftReleasedCallback;
        public static event EventHandler RightPressedCallback;
        public static event EventHandler RightReleasedCallback;
        public static event EventHandler MiddlePressedCallback;
        public static event EventHandler MiddleReleasedCallback;

        private static MouseState myPreviousMouseState;

        static public void Update()
        {
            MouseState statings = Mouse.GetState();
            if (myPreviousMouseState == null)
            {
                myPreviousMouseState = statings;
                return;
            }

            if (myPreviousMouseState.LeftButton == ButtonState.Released && statings.LeftButton == ButtonState.Pressed)
                LeftOnPressed();
            if (myPreviousMouseState.LeftButton == ButtonState.Pressed && statings.LeftButton == ButtonState.Released)
                LeftOnReleased();

            if (myPreviousMouseState.RightButton == ButtonState.Released && statings.RightButton == ButtonState.Pressed)
                RightOnPressed();
            if (myPreviousMouseState.RightButton == ButtonState.Pressed && statings.RightButton == ButtonState.Released)
                RightOnReleased();

            if (myPreviousMouseState.MiddleButton == ButtonState.Released && statings.MiddleButton == ButtonState.Pressed)
                MiddleOnPressed();
            if (myPreviousMouseState.MiddleButton == ButtonState.Pressed && statings.MiddleButton == ButtonState.Released)
                MiddleOnReleased();

            myPreviousMouseState = statings;
        }

        static private void LeftOnPressed()
        {
            LeftPressedCallback?.Invoke(null, null);
        }

        static private void LeftOnReleased()
        {
            LeftReleasedCallback?.Invoke(null, null);
        }

        static public bool LeftIsDown()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        static private void RightOnPressed()
        {
            RightPressedCallback?.Invoke(null, null);
        }

        static private void RightOnReleased()
        {
            RightReleasedCallback?.Invoke(null, null);
        }

        static public bool RightIsDown()
        {
            return Mouse.GetState().RightButton == ButtonState.Pressed;
        }

        static private void MiddleOnPressed()
        {
            MiddlePressedCallback?.Invoke(null, null);
        }

        static private void MiddleOnReleased()
        {
            MiddleReleasedCallback?.Invoke(null, null);
        }

        static public bool MiddleIsDown()
        {
            return Mouse.GetState().MiddleButton == ButtonState.Pressed;
        }
    }
}
