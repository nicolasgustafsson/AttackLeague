using AttackLeague.Utility.StateStack;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.GUI;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework;

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
            Button quickPlayButton = new Button();
            quickPlayButton.SetSprite("PlayButton", new Point(0, 512));
            quickPlayButton.OnClicked = CreateGameStaet;
            myGUICaretaker.AddGUI(quickPlayButton);


            Button lobbyButton = new Button();
            lobbyButton.SetSprite("LobbyButton", new Point(0, 128));
            lobbyButton.OnClicked = CreateLobbying;
            myGUICaretaker.AddGUI(lobbyButton);
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
