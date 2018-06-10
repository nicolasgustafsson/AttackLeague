using AttackLeague.AttackLeague.Feedback;
using AttackLeague.AttackLeague.GameInfo;
using AttackLeague.AttackLeague.Player;
using AttackLeague.Utility;
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
    class GameState : State
    {
        bool myIsHosting = true;

        string myIPToConnectTo = "";

        public GameState()
        {
            myIsHosting = true;
        }

        public GameState(string IP)
        {
            myIsHosting = false;
            myIPToConnectTo = IP;
        }

        public override void OnEnter()
        {
            LoadContent();
        }

        public void LoadContent()
        {
            //YLF SPECIAL START
            if (myIsHosting == false)
            {
                NetPeer newConnection = new NetPeer();
                newConnection.StartConnection(myIPToConnectTo);
                NetPoster.Instance.Connection = newConnection;

                while (newConnection.IsConnected() == false)
                    Thread.Sleep(1);

                GameInfo.GameInfo.myPlayers.Add(new RemotePlayer(new PlayerInfo(0, EInputType.Keyboard, "ylf")));
                GameInfo.GameInfo.myPlayers.Add(new NetPostingPlayer(new PlayerInfo(1, EInputType.Keyboard)));
            }
            else
            {
                //lägg dit nytt state som har detta vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
                NetHost host = new NetHost();
                host.StartListen();
                NetPoster.Instance.Connection = host;

                //wait til connect
                while (host.IsConnected() == false)
                    Thread.Sleep(1); //inte sova utan rendera animation grej
                //^^^^^^^^ ^^^   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ ^^^^^^^^^^^^^^^ ^^^^^                         ^^ ^^^^  ^^ ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

                GameInfo.GameInfo.myPlayers.Add(new NetPostingPlayer(new PlayerInfo(0, EInputType.Keyboard)));
                GameInfo.GameInfo.myPlayers.Add(new RemotePlayer(new PlayerInfo(1, EInputType.Keyboard, "ylf")));
            }

            foreach (var playerdesu in GameInfo.GameInfo.myPlayers)
            {
                playerdesu.Initialize();
            }

            GameInfo.GameInfo.SetAutomaticAttackOrder();
        }

        private void AddPlayer(EInputType aInputType)
        {
            PlayerInfo playerInfo = new PlayerInfo(GameInfo.GameInfo.myPlayerCount, aInputType);

            GameInfo.GameInfo.myPlayers.Add(new Player.Player(playerInfo));
        }

        protected override void Update()
        {
            base.Update();

            foreach (var player in GameInfo.GameInfo.myPlayers)
                player.Update();

            FeedbackManager.Update();
        }

        protected override void Draw(SpriteBatch aSpriteBatch)
        {
            base.Draw(aSpriteBatch);

            const int MagicTileSize = 48;
            foreach (var player in GameInfo.GameInfo.myPlayers)
                player.Draw(aSpriteBatch, MagicTileSize);

            FeedbackManager.Draw(aSpriteBatch);
        }

    }
}
