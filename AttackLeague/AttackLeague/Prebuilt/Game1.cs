using AttackLeague.AttackLeague;
using AttackLeague.AttackLeague.Grid;
using AttackLeague.Utility;
using AttackLeague.AttackLeague.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using AttackLeague.AttackLeague.GameInfo;
using AttackLeague.AttackLeague.Feedback;
using System.Net.Sockets;
using AttackLeague.Utility.Network;
using AttackLeague.Utility.Network.Messages;
using System.Threading;

namespace AttackLeague
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            GameInfo.myScreenSize.X = graphics.PreferredBackBufferWidth;
            GameInfo.myScreenSize.Y = graphics.PreferredBackBufferHeight;

            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

            Content.RootDirectory = "Content";

            ContentManagerInstance.Content = Content;
        }

        protected override void Initialize()
        {
            base.Initialize();

            /*
             * either peer or host
                    case "c":
                        Debug.Assert(myCurrentConnection == null);
                        NetPeer newConnection = new NetPeer();

                        if (consoleCommand.Count < 2)
                        {
                            newConnection.StartConnection("localhost");
                        }
                        else
                        {
                            newConnection.StartConnection(consoleCommand[1]);
                        }

                        myCurrentConnection = newConnection;
                        break;
                    case "l":
                        Debug.Assert(myCurrentConnection == null);

                        NetHost host = new NetHost();
                        host.StartListen();

                        myCurrentConnection = host;
                        break;

                    case "pretty":
                        myCurrentConnection.WriteMessage(new PrettyMessage());
                        break;
             
             */
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GamePadWrapper.UpdateAllGamePads();
            //for (int i = 0; i < (int)EInputType.Length; i++)
            //{
            //    EInputType input = (EInputType)i;
            //    if (GamePadWrapper.IsGamePadConnected(input))
            //    {
            //       //AddPlayer(input);
            //    }
            //}

            //wait til connect
            NetHost host = new NetHost();
            host.StartListen();
            NetPoster.Instance.Connection = host;

            while(host.IsConnected() == false)
                Thread.Sleep(1);

            GameInfo.myPlayers.Add(new NetPostingPlayer(new PlayerInfo(0, EInputType.Keyboard)));
            GameInfo.myPlayers.Add(new RemotePlayer(new PlayerInfo(1, EInputType.Keyboard, "ylf")));

            foreach (var playerdesu in GameInfo.myPlayers)
            {
                playerdesu.Initialize();
            }
            GameInfo.SetAutomaticAttackOrder();
        }

        private void AddPlayer(EInputType aInputType)
        {
            PlayerInfo playerInfo = new PlayerInfo(GameInfo.myPlayerCount, aInputType);

            GameInfo.myPlayers.Add(new Player(playerInfo));
        }

        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime)
        {
            KeyboardWrapper.UpdateState();
            GamePadWrapper.UpdateAllGamePads();
            FrameCounter.IncrementFrameCount();

            /*
             if (new input type found)
                CreatePlayer(the new input type);
                DoSomeSplashyUIToTellThePlayersAnotherOneHasJoinedAndStuff
            */

            foreach (var player in GameInfo.myPlayers)
                player.Update();

            FeedbackManager.Update();

            base.Update(gameTime);
            NetPostMaster.Master.ResolveMessages();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            const int MagicTileSize = 48;
            foreach (var player in GameInfo.myPlayers)
                player.Draw(spriteBatch, MagicTileSize);

            FeedbackManager.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        // ---

        //private void CreatePlayer(EInputType aDefaultInputType)
        //{
        //    PlayerInfo playerInfo = new PlayerInfo(myPlayers.Count, aDefaultInputType); 
        //    myPlayers.Add(new Player(playerInfo));
        //}
    }
}
