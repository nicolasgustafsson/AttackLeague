using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace AttackLeague.Utility.Input
{
    static class KeyboardWrapper
    {
        public static void UpdateState()
        {
            myFramesSinceLastPressed++;
            myPreviousState = myCurrentState;
            myCurrentState = Keyboard.GetState();
        }

        public static String GetPressedText()
        {
            String outString = "";

            var previousList = myPreviousState.GetPressedKeys().ToList();
            var currentList = myCurrentState.GetPressedKeys().ToList();
            currentList.RemoveAll(x => previousList.Contains(x));

            foreach(Keys key in currentList)
            {
                outString += key.ToString();
            }

            return outString;
        }

        public static bool KeyPressed(Keys aKey)
        {
            return myCurrentState.IsKeyDown(aKey) && myPreviousState.IsKeyUp(aKey);
        }

        public static bool KeyDown(Keys aKey)
        {
            return myCurrentState.IsKeyDown(aKey);
        }

        public static bool KeyReleased(Keys aKey)
        {
            return myCurrentState.IsKeyUp(aKey) && myPreviousState.IsKeyDown(aKey);
        }

        public static bool KeyUp(Keys aKey)
        {
            return myCurrentState.IsKeyUp(aKey);
        }

        public static bool KeyCooldown(Keys aKey)
        {
            if (KeyPressed(aKey))
            {
                myLastCooldownKeyPressed = aKey;
                myFramesSinceLastPressed = 0;
                return true;
            }
            else if (myLastCooldownKeyPressed == aKey && myFramesSinceLastPressed > myMagicFramesNumberStuff)
                return KeyDown(aKey);
            else
                return false;
        }

        private static int myMagicFramesNumberStuff = 20;
        private static int myFramesSinceLastPressed;
        private static Keys myLastCooldownKeyPressed;

        private static KeyboardState myCurrentState;
        private static KeyboardState myPreviousState;
    }
}
