using AttackLeague.Utility;
using AttackLeague.Utility.GUI;
using AttackLeague.Utility.StateStack;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.States
{
    class LobbyState : State
    {
        public LobbyState()
        {
            LoadContent();
        }

        void LoadContent()
        {
            //TextBox inputBox = new TextBox();
            //inputBox.OnEnterPressed += InputBoxEnterPressed;

            myGUICaretaker = JsonUtility.LoadJsonTyped("LobbyMenuGUI") as GUICaretaker;
            //myGUICaretaker = JsonUtility.LoadJson<GUICaretaker>("LobbyMenuGUI");
            //why null, whyyyyyyyyy ? do investigate pl0x. hello ylfs titta kod
            //myGUICaretaker.AddGUI(inputBox, "IpInputBox");

            JsonUtility.SaveJson("LobbyMenuGUI", myGUICaretaker);
            IPTextBox IPTextBox = myGUICaretaker.GetGUI("IpInputBox") as IPTextBox;
            IPTextBox.OnEnterPressed += InputBoxEnterPressed;

            myGUICaretaker.GetGUI<Button>("HostButton").OnClicked += CreateGameStaet;
        }

        void InputBoxEnterPressed(TextBox aBox)
        {
            System.Net.IPAddress address;

            if (!System.Net.IPAddress.TryParse(aBox.myText, out address))
                return;

            myStateStack.AddCommand(new StateCommand { myCommandType = EStateCommandType.Add, myStateType = EStateType.Major, myState = new GameState(aBox.myText) });
        }

        public bool CreateGameStaet()
        {
            myGUICaretaker.GetGUI<Text>("Hosthost").SetVisibility(EGUIVisibility.Visible);
            myStateStack.AddCommand(new StateCommand { myCommandType = EStateCommandType.Add, myStateType = EStateType.Major, myState = new GameState() });
            //CreateGameStaet SOME GUI FOR THE THING SAYiNG "host host"
            return true;
        }

        public override void OnEnter()
        {
            GameInfo.GameInfo.SetMouseVisibility(true);
        }

        public override void OnExit()
        {
            GameInfo.GameInfo.SetMouseVisibility(false);
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void Draw(SpriteBatch aSpriteBatch)
        {
            base.Draw(aSpriteBatch);
        }
    }
}
