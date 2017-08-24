using AttackLeague.AttackLeague;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AttackLeague
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Grid myGrid;
        private Player myPlayer;

        private bool myIsPaused = false;

        //private int myFrameCounter = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            ActionMapper.BindAction("Pause", Keys.Enter, KeyStatus.KeyPressed);
            ActionMapper.BindAction("StepOnce", Keys.OemPeriod, KeyStatus.KeyPressed);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myGrid = new Grid(Content);
            myPlayer = new Player(Content, myGrid);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardWrapper.UpdateState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                myGrid.GenerateGrid();
            }

            if (ActionMapper.ActionIsActive("Pause"))
            {
                myIsPaused = !myIsPaused;
            }

            if (myIsPaused == false)
            {
                myGrid.Update();
                myPlayer.Update();
            }
            else
            {
                if (ActionMapper.ActionIsActive("StepOnce"))
                {
                    myGrid.Update();
                    myPlayer.Update();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            myGrid.Draw(spriteBatch);
            const int MagicTileSize = 48;
            myPlayer.Draw(spriteBatch, MagicTileSize);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
