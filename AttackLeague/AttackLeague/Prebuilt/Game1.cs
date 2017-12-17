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

            Content.RootDirectory = "Content";

            ContentManagerInstance.Content = Content;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GamePadWrapper.UpdateAllGamePads();
            for (int i = 0; i < (int)EInputType.Length; i++)
            {
                EInputType input = (EInputType)i;
                if (GamePadWrapper.IsGamePadConnected(input))
                {
                    AddPlayer(input);
                }
            }

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

        protected override void UnloadContent()
        {

        }

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
