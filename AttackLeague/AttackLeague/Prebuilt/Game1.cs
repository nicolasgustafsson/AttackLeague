using AttackLeague.AttackLeague;
using AttackLeague.AttackLeague.Grid;
using AttackLeague.Utility;
using AttackLeague.AttackLeague.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using AttackLeague.AttackLeague.GameInfo;

namespace AttackLeague
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<Player> myPlayers;

        public Game1()
        {
            GameInfo.myPlayerCount = 0;

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            GameInfo.myScreenSize.X = graphics.PreferredBackBufferWidth;
            GameInfo.myScreenSize.Y = graphics.PreferredBackBufferHeight;

            Content.RootDirectory = "Content";

            ContentManagerInstance.Content = Content;

            myPlayers = new List<Player>();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameInfo.myPlayerCount++;
            //myPlayers.Add(new DebugPlayer());
            myPlayers.Add(new DebugPlayer());
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

            foreach (var player in myPlayers)
                player.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            const int MagicTileSize = 48;
            foreach (var player in myPlayers)
                player.Draw(spriteBatch, MagicTileSize);

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
