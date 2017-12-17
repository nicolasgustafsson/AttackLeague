using AttackLeague.AttackLeague.Grid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AttackLeague.Utility.Sprite;
using System;
using System.Collections.Generic;
using AttackLeague.AttackLeague.Blocks.Angry;
using AttackLeague.Utility;

namespace AttackLeague.AttackLeague.Player
{
    class Player
    {
        protected Sprite mySprite;
        protected Point myPosition = new Point(2,5);
        protected GridBundle myGridBundle;
        protected bool myIsPaused = false;
        public PlayerInfo myPlayerInfo;
        public List<int> myAttackOrder { get; private set; }
        protected List<AngryInfo> myQueuedAngryBlocks = new List<AngryInfo>();

        public Player(PlayerInfo aInfo)
        {
            myPlayerInfo = aInfo;
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
                {
                    ResolveAngryQueue();
                }
            }

            if (myPlayerInfo.myMappedActions.ActionIsActive("SwapBlocks"))
                myGridBundle.Behavior.SwapBlocksRight(myPosition);

            if (myPlayerInfo.myMappedActions.ActionIsActive("RaiseBlocks"))
                myGridBundle.Behavior.SetIsRaisingBlocks();
        }

        public bool CanBeAttacked()
        {
            return myGridBundle.Behavior.myIsDead == false; // todo : do crazy stuff
        }

        public void ReceiveAttack(AngryInfo aAngryInfo)
        {
            myQueuedAngryBlocks.Add(aAngryInfo);
        }

        private void ResolveAngryQueue() // todo, call
        {
            foreach (var angryInfo in myQueuedAngryBlocks)
            {
                int xPos = 0;
                if (angryInfo.mySize.X != myGridBundle.Container.GetInitialWidth())
                {
                    xPos = Randomizer.GlobalRandomizer.Next(2) == 0 ? 0 : (myGridBundle.Container.GetInitialWidth() - angryInfo.mySize.X);
                }
                Point position = new Point(xPos, myGridBundle.Container.GetCurrentHeight() + angryInfo.mySize.Y);
                AngryBlockBundle angryBundle = myGridBundle.Generator.CreateAngryBlockBundleAtPosition(position, angryInfo.mySize);
                myGridBundle.Behavior.AddAngryBundle(angryBundle);
            }
            myQueuedAngryBlocks.Clear();
        }

        private bool CanBeAtTop()
        {
            return myGridBundle.Container.IsExceedingRoof();
        }

        private void HandleMovement()
        {
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveRight"))
            { 
                if (myPosition.X < myGridBundle.Container.GetInitialWidth() - 2)
                    myPosition.X += 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveLeft"))
            {
                if (myPosition.X > 0)
                    myPosition.X -= 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveUp"))
            {
                if (myPosition.Y < myGridBundle.Container.GetInitialHeight() - (CanBeAtTop() ? 0.0f : 1.0f))
                    myPosition.Y += 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveDown"))
            {
                if (myPosition.Y > 1)
                    myPosition.Y -= 1;
            }
        }

        public void Draw(SpriteBatch aSpriteBatch, float aTileSize)
        {
               //(myPosition.Y >= myGridBundle.Container.GetInitialHeight() && myGridBundle.Behavior.GetRaisingOffset() != 0f) == false)
            if (myGridBundle.Behavior.HasRaisedGridThisFrame() && myPosition.Y < myGridBundle.Container.GetInitialHeight() - (CanBeAtTop() ? 0 : 1))// &&
            {
                myPosition.Y++;
                Console.WriteLine("player pos y: " + myPosition.Y);
            }

            myGridBundle.Behavior.Draw(aSpriteBatch);

            float invertedYPosition = myGridBundle.Container.GetInitialHeight() - myPosition.Y;
            float yPosition = invertedYPosition - 1 + myGridBundle.Behavior.GetRaisingOffset();
            mySprite.SetPosition(new Vector2(myPosition.X , yPosition) * aTileSize + myGridBundle.Behavior.GetOffset() + new Vector2(-1, -1));
            mySprite.Draw(aSpriteBatch);
        }
    }
}
