using AttackLeague.Utility;
using AttackLeague.Utility.GUI;
using AttackLeague.Utility.Network;
using AttackLeague.Utility.Network.Messages;
using AttackLeague.Utility.StateStack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.States
{
    public delegate void ConnectedCallback();

    class LobbyState : State
    {
        bool myIsNetworking = false;
        ConnectedCallback myConnectedCallback = null;

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

            // --------------------

            NetPeer newConnection = new NetPeer();
            newConnection.StartConnection(aBox.myText);
            NetPoster.Instance.Connection = newConnection;
            myIsNetworking = true;

            // --------------------

            //myGUICaretaker.GetGUI<Text>("Connectingy").SetVisibility(EGUIVisibility.Visible); says "connectingy..."
            myConnectedCallback = () =>
            {
                myStateStack.AddCommand(new StateCommand { myCommandType = EStateCommandType.Add, myStateType = EStateType.Major, myState = new GameState(false) });
            };
        }

        public bool CreateGameStaet()
        {
            NetHost host = new NetHost();
            host.StartListen();
            NetPoster.Instance.Connection = host;
            myIsNetworking = true;

            myGUICaretaker.GetGUI<Text>("Hosthost").SetVisibility(EGUIVisibility.Visible);

            myConnectedCallback = () => 
            {
                myStateStack.AddCommand(new StateCommand { myCommandType = EStateCommandType.Add, myStateType = EStateType.Major, myState = new GameState(true)});
            };
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

            if (myIsNetworking == true && NetPoster.Instance.Connection.IsConnected() == true)
            {
                myIsNetworking = false;
                myConnectedCallback();
            }
        }

        protected override void Draw(SpriteBatch aSpriteBatch)
        {
            Text hostText = myGUICaretaker.GetGUI<Text>("Hosthost");

            int seconds = DateTime.Now.Second;

            int hostCount = seconds % 10;

            hostText.myText = "";

            for(int i = 0; i < hostCount; i++)
            {
                hostText.myText += "host ";
            }
            base.Draw(aSpriteBatch);
        }
    }
}
