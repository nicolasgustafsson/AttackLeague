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
using AttackLeague.Utility.Input;

namespace AttackLeague
{
    /*
    * THE BIG DOINGS

   Lounge
      *has hostings
      *has friend book
      *find hosted game (server)
       popular replays (TV) & voting systems for them
       has friend search
   Lobby (servers)
      *invite friends to your hosted gaem
      *manual invite to your hosted game
      *start game when everyone ready
      *chat

       let your friends invite friends
       set handicap & attack orders / game mode etc

   Story unlocks some things
       AI has block/cloth/something => you unlock it

   Other modes rewards
       Achievements / Coins? Unlockables?


   More states:
       Dressroom
           character customization
       Thingroom
           block & effects customization
       Furnitureroom
           customize your own lobby? Because that's cool

   */

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
            EventInput.Initialize(Window);
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
