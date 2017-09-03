using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace AttackLeague.Utility
{
    enum KeyStatus
    {
        KeyDown,
        KeyPressed,
        KeyUp,
        KeyReleased
    }

    interface IAction
    {
        bool IsActive();
    }

    class KeyboardAction : IAction
    {
        private KeyStatus myKeyTriggerStatus;
        private Keys myKeyboardKey;

        public KeyboardAction(KeyStatus aKeyTriggerStatus, Keys aKey)
        {
            myKeyTriggerStatus = aKeyTriggerStatus;
            myKeyboardKey = aKey;
        }

        public bool IsActive()
        {
            switch (myKeyTriggerStatus)
            {
                case KeyStatus.KeyDown:
                    return KeyboardWrapper.KeyDown(myKeyboardKey);

                case KeyStatus.KeyPressed:
                    return KeyboardWrapper.KeyPressed(myKeyboardKey);

                case KeyStatus.KeyUp:
                    return KeyboardWrapper.KeyUp(myKeyboardKey);

                case KeyStatus.KeyReleased:
                    return KeyboardWrapper.KeyReleased(myKeyboardKey);

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
    static class ActionMapper
    {
        public static void BindAction(string aActionName, Keys aKey, KeyStatus aKeyStatus)
        {
            if (!myMappedActions.ContainsKey(aActionName))
            {
                myMappedActions[aActionName] = new List<IAction>();
            }
            myMappedActions[aActionName].Add(new KeyboardAction(aKeyStatus, aKey));
        }

        public static void UnbindAction(string aActionName)
        {
            myMappedActions.Remove(aActionName);
        }

        public static bool ActionIsActive(string aActionName)
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

        private static Dictionary<string, List<IAction>> myMappedActions = new Dictionary<string, List<IAction>>();
    }
}
