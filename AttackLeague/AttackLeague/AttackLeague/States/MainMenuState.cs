using AttackLeague.Utility.StateStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework;
using AttackLeague.AttackLeague.GameInfo;

namespace AttackLeague.AttackLeague.States
{
    class MainMenuState : State
    {
        Sprite myPlayButten;

        public MainMenuState()
        {
            LoadContent();
        }

        void LoadContent()
        {
            myPlayButten = new Sprite("PlayButton");

            myPlayButten.SetPosition(new Vector2(512, 512));
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
            myPlayButten.Draw(aSpriteBatch);
            base.Draw(aSpriteBatch);
        }
    }
}
