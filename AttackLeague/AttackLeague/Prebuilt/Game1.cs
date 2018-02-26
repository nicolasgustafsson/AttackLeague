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
using AttackLeague.Utility.StateStack;
using AttackLeague.AttackLeague.States;
using AttackLeague.Utility.GUI;

namespace AttackLeague
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager myGraphicsDeviceManager;
        private SpriteBatch mySpriteBatch;
        private StateStack myStateStack = new StateStack();

        public Game1()
        {
            myGraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            GameInfo.myMouseFunction = SetMouseVisibility;
            GameInfo.myScreenSize.X = myGraphicsDeviceManager.PreferredBackBufferWidth;
            GameInfo.myScreenSize.Y = myGraphicsDeviceManager.PreferredBackBufferHeight;

            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

            Content.RootDirectory = "Content";

            ContentManagerInstance.Content = Content;
        }

        protected override void Initialize()
        {
            myStateStack.AddCommand(new StateCommand { myCommandType = EStateCommandType.Add, myStateType = EStateType.Major, myState = new MainMenuState() });
            base.Initialize();
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);
            GamePadWrapper.UpdateAllGamePads();
        }

        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime)
        {
            KeyboardWrapper.UpdateState();
            GamePadWrapper.UpdateAllGamePads();
            MouseUtility.Update();
            FrameCounter.IncrementFrameCount();

            myStateStack.ResolveQueuedThings();

            if (myStateStack.GetCurrentState() == null)
            {
                Exit();
                return;
            }

            myStateStack.Update();

            base.Update(gameTime);
            NetPostMaster.Master.ResolveMessages();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            mySpriteBatch.Begin();

            myStateStack.Draw(mySpriteBatch);

            mySpriteBatch.End();
            base.Draw(gameTime);
        }

        //-------------------------

        private void SetMouseVisibility(bool aVisibility)
        {
            IsMouseVisible = aVisibility;
        }
    }
}
