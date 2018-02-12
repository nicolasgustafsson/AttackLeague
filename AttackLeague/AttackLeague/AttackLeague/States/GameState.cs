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
        public GameState()
        {

        }

        public override void OnEnter()
        {
            LoadContent();
        }

        public void LoadContent()
        {
            //wait til connect
            NetHost host = new NetHost();
            host.StartListen();
            NetPoster.Instance.Connection = host;

            while (host.IsConnected() == false)
                Thread.Sleep(1);


            GameInfo.GameInfo.myPlayers.Add(new NetPostingPlayer(new PlayerInfo(0, EInputType.Keyboard)));
            GameInfo.GameInfo.myPlayers.Add(new RemotePlayer(new PlayerInfo(1, EInputType.Keyboard, "ylf")));

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

        public override void Update()
        {
            foreach (var player in GameInfo.GameInfo.myPlayers)
                player.Update();

            FeedbackManager.Update();
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            const int MagicTileSize = 48;
            foreach (var player in GameInfo.GameInfo.myPlayers)
                player.Draw(aSpriteBatch, MagicTileSize);

            FeedbackManager.Draw(aSpriteBatch);
        }

    }
}
