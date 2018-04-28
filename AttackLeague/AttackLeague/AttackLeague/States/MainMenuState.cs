using AttackLeague.Utility.StateStack;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.GUI;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework;
using AttackLeague.Utility;
using System.IO;

namespace AttackLeague.AttackLeague.States
{
    class MainMenuState : State
    {
        public MainMenuState()
        {
            LoadContent();
        }

        void LoadContent()
        {
            myGUICaretaker = JsonUtility.LoadJson<GUICaretaker>("MainMenuGUI");

            myGUICaretaker.GetButton("MainMenuPlay").OnClicked += CreateGameStaet;
            myGUICaretaker.GetButton("LobbyButton").OnClicked += CreateLobbying;
        }

        public bool CreateGameStaet()
        {
            myStateStack.AddCommand(new StateCommand { myCommandType = EStateCommandType.Add, myStateType = EStateType.Major, myState = new GameState() });
            return true;
        }

        public bool CreateLobbying()
        {
            myStateStack.AddCommand(new StateCommand { myCommandType = EStateCommandType.Add, myStateType = EStateType.Major, myState = new LobbyState() });
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
    }
}
