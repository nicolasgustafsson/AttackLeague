using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace AttackLeague.Utility
{
    enum InputStatus
    {
        KeyDown,
        KeyPressed,
        KeyUp,
        KeyReleased,
        KeyCooldown // TODO RENAME
    }

    interface IAction
    {
        bool IsActive();
    }

    class KeyboardAction : IAction
    {
        private InputStatus myKeyTriggerStatus;
        private Keys myKeyboardKey;

        public KeyboardAction(InputStatus aKeyTriggerStatus, Keys aKey)
        {
            myKeyTriggerStatus = aKeyTriggerStatus;
            myKeyboardKey = aKey;
        }

        public bool IsActive()
        {
            switch (myKeyTriggerStatus)
            {
                case InputStatus.KeyDown:
                    return KeyboardWrapper.KeyDown(myKeyboardKey);

                case InputStatus.KeyPressed:
                    return KeyboardWrapper.KeyPressed(myKeyboardKey);

                case InputStatus.KeyUp:
                    return KeyboardWrapper.KeyUp(myKeyboardKey);

                case InputStatus.KeyReleased:
                    return KeyboardWrapper.KeyReleased(myKeyboardKey);

                case InputStatus.KeyCooldown:
                    return KeyboardWrapper.KeyCooldown(myKeyboardKey);

                default:
                    return false;
            }
        }
    }

    class GamePadAction : IAction
    {
        private InputStatus myButtonTriggerStatus; 
        private Buttons myButton;
        private int myControllerIndex;

        public GamePadAction(InputStatus aButtonTriggerStatus, Buttons aButton, int aControllerIndex)
        {
            myControllerIndex = aControllerIndex;
            myButtonTriggerStatus = aButtonTriggerStatus;
            myButton = aButton;
        }

        public bool Connected()
        {
            return GamePad.GetCapabilities(myControllerIndex).IsConnected;
        }

        public bool IsActive()
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(myControllerIndex);
            if (capabilities.IsConnected == false)
                return false;

            switch (myButtonTriggerStatus)
            {
                case InputStatus.KeyDown:
                    return GamePadWrapper.ButtonDown(myButton, myControllerIndex);

                case InputStatus.KeyPressed:
                    return GamePadWrapper.ButtonPressed(myButton, myControllerIndex);

                case InputStatus.KeyUp:
                    return GamePadWrapper.ButtonUp(myButton, myControllerIndex);

                case InputStatus.KeyReleased:
                    return GamePadWrapper.ButtonReleased(myButton, myControllerIndex);

                case InputStatus.KeyCooldown:
                    return GamePadWrapper.ButtonCooldown(myButton, myControllerIndex);

                default:
                    return false;
            }
        }
    }

    #region RecordingActions


    class RecordedAction : IAction
    {
        List<int> Frames;


        public bool IsActive()
        {
            return true;
        }
    }


    #endregion
    /// <summary>
    /// Maps actions to input
    /// </summary>
    class ActionMapper
    {
        public ActionMapper()
        {
            myMappedActions = new Dictionary<string, List<IAction>>();
        }

        public void BindAction(string aActionName, Buttons aButton, InputStatus aButtonTriggerStatus, int aControllerIndex)
        {
            if (!myMappedActions.ContainsKey(aActionName))
            {
                myMappedActions[aActionName] = new List<IAction>();
            }
            myMappedActions[aActionName].Add(new GamePadAction(aButtonTriggerStatus, aButton, aControllerIndex));
        }

        public void BindAction(string aActionName, Keys aKey, InputStatus aKeyStatus)
        {
            if (!myMappedActions.ContainsKey(aActionName))
            {
                myMappedActions[aActionName] = new List<IAction>();
            }
            myMappedActions[aActionName].Add(new KeyboardAction(aKeyStatus, aKey));
        }

        public void UnbindAction(string aActionName)
        {
            myMappedActions.Remove(aActionName);
        }

        public bool ActionIsActive(string aActionName)
        {
            Debug.Assert(myMappedActions.ContainsKey(aActionName), "Action " + aActionName + " not bound!");

            foreach (IAction action in myMappedActions[aActionName])
            {
                if (action.IsActive())
                {
                    return true;
                }
            }

            return false;
        }

        private Dictionary<string, List<IAction>> myMappedActions;
    }
}
