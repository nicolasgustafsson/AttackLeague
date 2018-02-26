using AttackLeague.Utility.StateStack;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.GUI;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework;

namespace AttackLeague.AttackLeague.States
{
    class MainMenuState : State
    {
        Button myButtonyButton;

        public MainMenuState()
        {
            LoadContent();
        }

        void LoadContent()
        {
            //myPlayButten = new Sprite("PlayButton");
            //myPlayButten.SetPosition(new Vector2(512, 512));

            myButtonyButton = new Button();
            myButtonyButton.mySprite = new Sprite("PlayButton");
            myButtonyButton.mySprite.SetPosition(new Vector2(512, 512));
            myButtonyButton.myHotspot = new Microsoft.Xna.Framework.Rectangle(512, 512, 200, 200);
            myButtonyButton.OnClicked = CreateGameStaet;
        }

        public bool CreateGameStaet()
        {
            myStateStack.AddCommand(new StateCommand { myCommandType = EStateCommandType.Add, myStateType = EStateType.Major, myState = new GameState() });
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
            myButtonyButton.mySprite.Draw(aSpriteBatch);
            base.Draw(aSpriteBatch);
        }
    }
}
