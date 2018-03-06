using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using AttackLeague.AttackLeague.Player;

namespace AttackLeague.Utility.Input
{
    static class GamePadWrapper
    {
        public static void UpdateAllGamePads()
        {
            for (int i = 0; i < myControllerStates.Count(); ++i)
                UpdateState(i);
        }

        public static void UpdateState(int aControllerIndex)
        {
            myControllerStates[aControllerIndex].myFramesSinceLastPressed++;
            myControllerStates[aControllerIndex].myPreviousState = myControllerStates[aControllerIndex].myCurrentState;
            myControllerStates[aControllerIndex].myCurrentState = GamePad.GetState(aControllerIndex);
        }

        public static bool IsGamePadConnected(EInputType aType)
        {
            if (aType >= EInputType.Length)
                return false;
            if (aType == EInputType.Keyboard)
                return true;

            return myControllerStates[(int)aType].myCurrentState.IsConnected;
        }

        public static bool ButtonPressed(Buttons aButton, int aControllerIndex)
        {
            return myControllerStates[aControllerIndex].myCurrentState.IsButtonDown(aButton) &&
                   myControllerStates[aControllerIndex].myPreviousState.IsButtonUp(aButton);
        }

        public static bool ButtonDown(Buttons aButton, int aControllerIndex)
        {
            return myControllerStates[aControllerIndex].myCurrentState.IsButtonDown(aButton);
        }

        public static bool ButtonReleased(Buttons aButton, int aControllerIndex)
        {
            return myControllerStates[aControllerIndex].myCurrentState.IsButtonUp(aButton) &&
                   myControllerStates[aControllerIndex].myPreviousState.IsButtonDown(aButton);
        }

        public static bool ButtonUp(Buttons aButton, int aControllerIndex)
        {
            return myControllerStates[aControllerIndex].myCurrentState.IsButtonUp(aButton);
        }

        public static bool ButtonCooldown(Buttons aButton, int aControllerIndex)
        {
            if (ButtonPressed(aButton, aControllerIndex))
            {
                myControllerStates[aControllerIndex].myLastCooldownButtonPressed = aButton;
                myControllerStates[aControllerIndex].myFramesSinceLastPressed = 0;
                return true;
            }
            else if (myControllerStates[aControllerIndex].myLastCooldownButtonPressed == aButton && myControllerStates[aControllerIndex].myFramesSinceLastPressed > myMagicFramesNumberStuff)
                return ButtonDown(aButton, aControllerIndex);
            else
                return false;
        }

        private static int myMagicFramesNumberStuff = 20;

        private static ControllerInfo[] myControllerStates = new ControllerInfo[4] { new ControllerInfo(), new ControllerInfo(), new ControllerInfo(), new ControllerInfo() };

        internal class ControllerInfo
        {
            public GamePadState myCurrentState;
            public GamePadState myPreviousState;

            public int myFramesSinceLastPressed;
            public Buttons myLastCooldownButtonPressed;

            bool JustDisconnected() 
            {
                return myCurrentState.IsConnected == false && myPreviousState.IsConnected == true;
            }

            bool JustConnected()
            {
                return myCurrentState.IsConnected == true && myPreviousState.IsConnected == false;
            }
        }

    }
}
