using AttackLeague.AttackLeague.Grid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.Sprites;
using System;
using System.Collections.Generic;
using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.Utility;
using System.Diagnostics;
using AttackLeague.Utility.Network.Messages;
using AttackLeague.AttackLeague.Feedback;

namespace AttackLeague.AttackLeague.Player
{
    class Player
    {
        /*
         * TWO BUGS: If lag is above 100 frames, we crashy crashy
         * We desync after long session
         * Have fun! :D :D :D :D :D
         * 
        båda spelar
        en skickar block på den andra
        det laggar
        blocket skulle ramlat ner frame x

        x => block till y
        angryblockmessage
        y spawns block and sends in its update
        x show block spawned at frame x
         */
        protected Sprite mySprite;
        protected Point myPosition = new Point(2,5);
        protected GridBundle myGridBundle;
        protected bool myIsPaused = false;
        public PlayerInfo myPlayerInfo;
        public List<int> myAttackOrder { get; private set; }
        protected List<AngryInfo> myQueuedAngryBlocks = new List<AngryInfo>();

        protected int myElapsedFrames = 0;

        public Player(PlayerInfo aInfo)
        {
            myPlayerInfo = aInfo;
            /*
             * Frame counter
             * Send angry block will resolve 100 frames 
             */
        }

        public void Initialize()
        {
            myAttackOrder = new List<int>();
            myGridBundle = new GridBundle(myPlayerInfo.myPlayerIndex);
            //myGridBehavior = new GridBehavior(myPlayerInfo.myPlayerIndex); //gridcontainer?
            mySprite = new Sprite("curse");
        }

        public virtual void Update()
        {
            HandleMovement();

            if (myIsPaused == false)
            {
                myGridBundle.Behavior.Update();

                if (myGridBundle.Behavior.IsFrozen() == false)
                    ResolveAngryQueue();
            }

            HandleActions();

            if (myGridBundle.Behavior.HasRaisedGridThisFrame() && myPosition.Y < myGridBundle.Container.GetInitialHeight() - (CanBeAtTop() ? 0 : 1))// &&
            {
                myPosition.Y++;
            }

            myElapsedFrames++;
        }

        protected virtual void HandleActions()
        {
            if (myPlayerInfo.myMappedActions.ActionIsActive(ActionList.SwapBlocks))
                myGridBundle.Behavior.SwapBlocksRight(myPosition);

            if (myPlayerInfo.myMappedActions.ActionIsActive(ActionList.RaiseBlocks))
                myGridBundle.Behavior.SetIsRaisingBlocks();
        }

        public bool CanBeAttacked()
        {
            return myGridBundle.Behavior.myIsDead == false; // todo : do crazy stuff
        }

        public virtual void DebugDied(int aPlayerIndex)
        {
            if (aPlayerIndex != myPlayerInfo.myPlayerIndex)
                return;
            Vector2 smallOffset = new Vector2();
            smallOffset.X = myGridBundle.Container.GetInitialWidth() / 3 * AbstractBlock.GetTileSize();
            smallOffset.Y = myGridBundle.Container.GetInitialHeight() / 3 * AbstractBlock.GetTileSize();
            SpriteFeedback losar = new SpriteFeedback("fantastiskt", myGridBundle.Behavior.GetOffset() + smallOffset, () =>
            {
                return myGridBundle.Behavior.myIsDead == false;
            });
            FeedbackManager.AddFeedback(losar);
        }

        public virtual void ReceiveAttack(AngryInfo aAngryInfo)
        {
            myQueuedAngryBlocks.Add(aAngryInfo);
        }

        public int GetElapsedFrames()
        {
            return myElapsedFrames;
        }

        protected virtual void ResolveAngryQueue() // todo, call (old?)
        {
            foreach (var angryInfo in myQueuedAngryBlocks)
            {
                int xPos = 0;
                if (angryInfo.mySizeX != myGridBundle.Container.GetInitialWidth())
                {
                    xPos = myGridBundle.GridRandomizer.Next(2) == 0 ? 0 : (myGridBundle.Container.GetInitialWidth() - angryInfo.mySizeX);
                }

                Point position = new Point(xPos, myGridBundle.Container.GetCurrentHeight() + angryInfo.mySizeY);
                AngryBlockBundle angryBundle = myGridBundle.Generator.CreateAngryBlockBundleAtPosition(position, angryInfo.GetSize());
                myGridBundle.Behavior.AddAngryBundle(angryBundle);

                if (this is RemotePlayer)
                {
                    Console.WriteLine($"Resolve at {myElapsedFrames}");
                }
            }

            myQueuedAngryBlocks.Clear();
        }

        protected bool CanBeAtTop()
        {
            return myGridBundle.Container.IsExceedingRoof();
        }

        protected virtual void HandleMovement()
        {
            if (myPlayerInfo.myMappedActions.ActionIsActive(ActionList.MoveRight))
            { 
                if (myPosition.X < myGridBundle.Container.GetInitialWidth() - 2)
                    myPosition.X += 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive(ActionList.MoveLeft))
            {
                if (myPosition.X > 0)
                    myPosition.X -= 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive(ActionList.MoveUp))
            {
                if (myPosition.Y < myGridBundle.Container.GetInitialHeight() - (CanBeAtTop() ? 0.0f : 1.0f))
                    myPosition.Y += 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive(ActionList.MoveDown))
            {
                if (myPosition.Y > 1)
                    myPosition.Y -= 1;
            }
        }

        public void Draw(SpriteBatch aSpriteBatch, float aTileSize)
        {
               //(myPosition.Y >= myGridBundle.Container.GetInitialHeight() && myGridBundle.Behavior.GetRaisingOffset() != 0f) == false)


            myGridBundle.Behavior.Draw(aSpriteBatch);

            float invertedYPosition = myGridBundle.Container.GetInitialHeight() - myPosition.Y;
            float yPosition = invertedYPosition - 1 + myGridBundle.Behavior.GetRaisingOffset();
            mySprite.SetPosition(new Vector2(myPosition.X , yPosition) * aTileSize + myGridBundle.Behavior.GetOffset() + new Vector2(-1, -1));
            mySprite.Draw(aSpriteBatch);
        }
    }
}
