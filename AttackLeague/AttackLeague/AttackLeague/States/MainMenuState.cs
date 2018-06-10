using AttackLeague.Utility.StateStack;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.GUI;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework;
using AttackLeague.Utility;
using System.IO;
using System;

namespace AttackLeague.AttackLeague.States
{
    [Serializable]
    class TotallyArray
    {
        public TotallyArray()
        {
            myPosition = new Vector2();
            myPosition.X = 1;
            myPosition.Y = 5;
        }

        public Vector2 myPosition;
    }

    class MainMenuState : State
    {
        public MainMenuState()
        {
            LoadContent();
        }

        void LoadContent()
        {
            JsonUtility.SaveJson("EnMassaTotaltKnaprig", new TotallyArray());
            //int br = 0;
            //br++;

            //myGUICaretaker = JsonUtility.LoadJsonTyped("MainMenuGUI") as GUICaretaker; // /*LoadJson*/
            myGUICaretaker = JsonUtility.LoadJsonTyped("MainMenuGUI") as GUICaretaker; // LoadJson

            myGUICaretaker.GetGUI<Button>("MainMenuPlay").OnClicked += CreateGameStaet;
            myGUICaretaker.GetGUI<Button>("LobbyButton").OnClicked += CreateLobbying;

            JsonUtility.SaveJson("MainMenuGUI", myGUICaretaker);
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
