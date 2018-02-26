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
        TextBox myInputBox;

        public LobbyState()
        {
            LoadContent();
        }

        void LoadContent()
        {
            myInputBox = new TextBox();
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
            myInputBox.Draw(aSpriteBatch);
            base.Draw(aSpriteBatch);
        }
    }
}
