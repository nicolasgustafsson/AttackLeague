using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;
using Microsoft.Xna.Framework.Input;
using AttackLeague.Utility.Input;

namespace AttackLeague.AttackLeague.Player
{
    enum EInputType
    {
        GamePad1,
        GamePad2,
        GamePad3,
        GamePad4,
        Keyboard,
        Networked,
        Length
    }

    //information regarding the player playing the player ingame
    class PlayerInfo
    {
        public string myName;
        public EInputType myInputType;
        public int myPlayerIndex;
        public ActionMapper myMappedActions;

        public PlayerInfo(int aPlayerIndex, EInputType aInputType, string aPlayerName = null)
        {
            myPlayerIndex = aPlayerIndex;
            myInputType = aInputType;
            myMappedActions = new ActionMapper();

            if (aPlayerName == null)
            {
                GenerateName();
                GenerateDefaultActions();
            }
            else
            {
                myName = aPlayerName;
                GenerateDefaultActions();
                // json read mapped actions, if new player, generate default anyways
            }
        }

        private void GenerateName()
        {
            myName = Names[Randomizer.GlobalRandomizer.Next(Names.Length)];

            Console.WriteLine(myName);
        }

        protected virtual void GenerateDefaultActions()
        {
            switch (myInputType)
            {
                case EInputType.GamePad1:
                case EInputType.GamePad2:
                case EInputType.GamePad3:
                case EInputType.GamePad4:
                    GenerateDefaultGamePadActions();
                    break;
                case EInputType.Keyboard:
                    GenerateDefaultKeyboardActions();
                    break;
                default:
                    break;
            }
        }

        protected void GenerateDefaultGamePadActions()
        {
            int controllerIndex = (int)myInputType;
            myMappedActions.BindAction("MoveLeft", Buttons.DPadLeft, InputStatus.KeyCooldown, controllerIndex);
            myMappedActions.BindAction("MoveLeft", Buttons.LeftThumbstickLeft, InputStatus.KeyCooldown, controllerIndex);

            myMappedActions.BindAction("MoveRight", Buttons.DPadRight, InputStatus.KeyCooldown, controllerIndex);
            myMappedActions.BindAction("MoveRight", Buttons.LeftThumbstickRight, InputStatus.KeyCooldown, controllerIndex);

            myMappedActions.BindAction("MoveUp", Buttons.DPadUp, InputStatus.KeyCooldown, controllerIndex);
            myMappedActions.BindAction("MoveUp", Buttons.LeftThumbstickUp, InputStatus.KeyCooldown, controllerIndex);

            myMappedActions.BindAction("MoveDown", Buttons.DPadDown, InputStatus.KeyCooldown, controllerIndex);
            myMappedActions.BindAction("MoveDown", Buttons.LeftThumbstickDown, InputStatus.KeyCooldown, controllerIndex);

            myMappedActions.BindAction("SwapBlocks", Buttons.A, InputStatus.KeyPressed, controllerIndex);
            myMappedActions.BindAction("SwapBlocks", Buttons.B, InputStatus.KeyPressed, controllerIndex);

            myMappedActions.BindAction("RaiseBlocks", Buttons.RightShoulder, InputStatus.KeyDown, controllerIndex);
            myMappedActions.BindAction("RaiseBlocks", Buttons.RightTrigger, InputStatus.KeyDown, controllerIndex);

            myMappedActions.BindAction("RaiseBlocks", Buttons.LeftShoulder, InputStatus.KeyDown, controllerIndex);
            myMappedActions.BindAction("RaiseBlocks", Buttons.LeftTrigger, InputStatus.KeyDown, controllerIndex);

            myMappedActions.BindAction("Back", Buttons.B, InputStatus.KeyPressed, controllerIndex);
            myMappedActions.BindAction("Confirm", Buttons.A, InputStatus.KeyPressed, controllerIndex);
            myMappedActions.BindAction("Menu", Buttons.Start, InputStatus.KeyPressed, controllerIndex);
        }

        protected void GenerateDefaultKeyboardActions()
        {
            myMappedActions.BindAction("MoveLeft", Keys.A, InputStatus.KeyCooldown);
            myMappedActions.BindAction("MoveLeft", Keys.Left, InputStatus.KeyCooldown);

            myMappedActions.BindAction("MoveRight", Keys.D, InputStatus.KeyCooldown);
            myMappedActions.BindAction("MoveRight", Keys.Right, InputStatus.KeyCooldown);

            myMappedActions.BindAction("MoveUp", Keys.W, InputStatus.KeyCooldown);
            myMappedActions.BindAction("MoveUp", Keys.Up, InputStatus.KeyCooldown);

            myMappedActions.BindAction("MoveDown", Keys.S, InputStatus.KeyCooldown);
            myMappedActions.BindAction("MoveDown", Keys.Down, InputStatus.KeyCooldown);

            myMappedActions.BindAction("SwapBlocks", Keys.E, InputStatus.KeyPressed);
            myMappedActions.BindAction("RaiseBlocks", Keys.Space, InputStatus.KeyDown);

            myMappedActions.BindAction("Back", Keys.Escape, InputStatus.KeyPressed);
            myMappedActions.BindAction("Confirm", Keys.Enter, InputStatus.KeyPressed);
        }

        static string[] Names = new string[]
        {
            "Ylf",
            "Nicos",
            "Dan",
            "Victor",
            "Toberl",
            "L1",
            "Erik",
            "Lasses",
            "Robin",
            "Holst",
            "Jonas",
            "Spain",
            "Pidda",
            "Leon",
            "Mattias",
            "Tobbelin",
            "Paprika",
            "Emil",
            "Falck",
            "Findus",
            "Håkan",
            "Kevin",
            "Olle"
        };
    }

}
