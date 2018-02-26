using AttackLeague.Utility.StateStack;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.GUI;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework;

namespace AttackLeague.AttackLeague.States
{
    class MainMenuState : State
    {
        Button myQuickPlayButton;
        Button myLobbyButton;

        public MainMenuState()
        {
            LoadContent();
        }

        void LoadContent()
        {
            myQuickPlayButton = new Button();
            myQuickPlayButton.SetSprite("PlayButton", new Point(0, 512));
            myQuickPlayButton.OnClicked = CreateGameStaet;

            myLobbyButton = new Button();
            myLobbyButton.SetSprite("LobbyButton", new Point(0, 128));
            myLobbyButton.OnClicked = CreateLobbying;
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

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            myQuickPlayButton.Draw(aSpriteBatch);
            myLobbyButton.Draw(aSpriteBatch);
            base.Draw(aSpriteBatch);
        }
    }
}
