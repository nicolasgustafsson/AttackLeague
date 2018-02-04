using AttackLeague.AttackLeague.Grid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.Sprite;
using System;
using System.Collections.Generic;
using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.Utility;
using System.Diagnostics;

namespace AttackLeague.AttackLeague.Player
{
    class Player
    {
        /*
         * TWO BUGS: If lag is above 100 frames, we crashy crashy
         * We desync after long session
         * Have fun, 
         */
        protected Sprite mySprite;
        protected Point myPosition = new Point(2,5);
        protected GridBundle myGridBundle;
        protected bool myIsPaused = false;
        public PlayerInfo myPlayerInfo;
        public List<int> myAttackOrder { get; private set; }
        protected List<AngryInfo> myQueuedAngryBlocks = new List<AngryInfo>();
        protected List<AngryInfo> myQueueQueueAngryBlocks = new List<AngryInfo>();

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

                int resolvedCount = 0;
                foreach (var queueQueueAngryInfo in myQueueQueueAngryBlocks)
                {
                    Debug.Assert(queueQueueAngryInfo.myFrameIndexToResolve >= myElapsedFrames);
                    if (queueQueueAngryInfo.myFrameIndexToResolve == myElapsedFrames)
                    {
                        resolvedCount++;
                        myQueuedAngryBlocks.Add(queueQueueAngryInfo);
                    }
                    else
                        break;
                }

                for (int i = 0; i < resolvedCount; i++)
                    myQueueQueueAngryBlocks.RemoveAt(0);


                if (myGridBundle.Behavior.IsFrozen() == false)
                {
                    ResolveAngryQueue();
                }
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

        public void ReceiveAttack(AngryInfo aAngryInfo)
        {
            myQueueQueueAngryBlocks.Add(aAngryInfo);
        }

        public int GetElapsedFrames()
        {
            return myElapsedFrames;
        }

        protected void ResolveAngryQueue() // todo, call (old?)
        {
            foreach (var angryInfo in myQueuedAngryBlocks)
            {
                int xPos = 0;
                if (angryInfo.mySize.X != myGridBundle.Container.GetInitialWidth())
                {
                    xPos = myGridBundle.GridRandomizer.Next(2) == 0 ? 0 : (myGridBundle.Container.GetInitialWidth() - angryInfo.mySize.X);
                }
                Point position = new Point(xPos, myGridBundle.Container.GetCurrentHeight() + angryInfo.mySize.Y);
                AngryBlockBundle angryBundle = myGridBundle.Generator.CreateAngryBlockBundleAtPosition(position, angryInfo.mySize);
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
