﻿using AttackLeague.Utility.GUI;
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
            TextBox inputBox = new TextBox();
            inputBox.OnEnterPressed += InputBoxEnterPressed;
            myGUICaretaker.AddGUI(inputBox);
        }

        void InputBoxEnterPressed(TextBox aBox)
        {
            Console.WriteLine(aBox.myText);
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