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
    enum EMatchType
    {
        Host,
        Client,
        Singeplayer
    }

    struct MatchInfo
    {
        public List<PlayerInfo> myPlayers;
        public EMatchType myMatchType;
    }

    class GameState : State
    {
        private MatchInfo myMatchInfo;

        //string myIPToConnectTo = "";

        public GameState(MatchInfo aMatchInfo)
        {
            myMatchInfo = aMatchInfo;
        }

        //public GameState(string IP)
        //{
        //    myIsHosting = false;
        //    myIPToConnectTo = IP;
        //}

        public override void OnEnter()
        {
            LoadContent();
        }

        public void LoadContent()
        {
            //YLF SPECIAL START

            switch (myMatchInfo.myMatchType)
            {
                case EMatchType.Host:
                    GameInfo.GameInfo.myPlayers.Add(new NetPostingPlayer(new PlayerInfo(0, EInputType.Keyboard)));
                    GameInfo.GameInfo.myPlayers.Add(new RemotePlayer(new PlayerInfo(1, EInputType.Networked, "ylf")));
                    break;

                case EMatchType.Client:
                    GameInfo.GameInfo.myPlayers.Add(new RemotePlayer(new PlayerInfo(0, EInputType.Networked, "ylf")));
                    GameInfo.GameInfo.myPlayers.Add(new NetPostingPlayer(new PlayerInfo(1, EInputType.Keyboard)));
                    break;

                case EMatchType.Singeplayer:
                    GameInfo.GameInfo.myPlayers.Add(new DebugPlayer());
                    break;
            }
            //if (myIsHosting == false)
            //{
            //    GameInfo.GameInfo.myPlayers.Add(new RemotePlayer(new PlayerInfo(0, EInputType.Networked, "ylf")));
            //    GameInfo.GameInfo.myPlayers.Add(new NetPostingPlayer(new PlayerInfo(1, EInputType.Keyboard)));
            //}
            //else
            //{
            //    GameInfo.GameInfo.myPlayers.Add(new NetPostingPlayer(new PlayerInfo(0, EInputType.Keyboard)));
            //    GameInfo.GameInfo.myPlayers.Add(new RemotePlayer(new PlayerInfo(1, EInputType.Networked, "ylf")));
            //}

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
