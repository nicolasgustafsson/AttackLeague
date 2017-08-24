using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace AttackLeague.Utility
{
    static class KeyboardWrapper
    {
        public static void UpdateState()
        {
            myPreviousState = myCurrentState;
            myCurrentState = Keyboard.GetState();
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

        private static KeyboardState myCurrentState;
        private static KeyboardState myPreviousState;
    }
}
