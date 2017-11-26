using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;
using AttackLeague.AttackLeague.Grid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace AttackLeague.AttackLeague.Player
{
    class Player
    {
        protected Sprite mySprite;
        protected Point myPosition = new Point(2,5);
        protected GridBehavior myGridBehavior;
        protected bool myIsPaused = false;
        public PlayerInfo myPlayerInfo;

        public Player(PlayerInfo aInfo)
        {
            myPlayerInfo = aInfo;

            myGridBehavior = new GridBehavior(myPlayerInfo.myPlayerIndex); //gridcontainer?
            mySprite = new Sprite("curse");
        }

        public virtual void Update()
        {
            HandleMovement();

            if (myIsPaused == false)
                myGridBehavior.Update();

            if (myPlayerInfo.myMappedActions.ActionIsActive("SwapBlocks"))
                myGridBehavior.SwapBlocksRight(myPosition);

            if (myPlayerInfo.myMappedActions.ActionIsActive("RaiseBlocks"))
                myGridBehavior.SetIsRaisingBlocks();
        }

        private void HandleMovement()
        {
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveRight"))
            { 
                if (myPosition.X < myGridBehavior.GetWidth() - 2)
                    myPosition.X += 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveLeft"))
            {
                if (myPosition.X > 0)
                    myPosition.X -= 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveUp"))
            {
                if (myPosition.Y < myGridBehavior.GetHeight())
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
            if (myGridBehavior.HasRaisedGridThisFrame())
                myPosition.Y++;

            myGridBehavior.Draw(aSpriteBatch);

            float invertedYPosition = myGridBehavior.GetHeight() - myPosition.Y;
            float yPosition = invertedYPosition - 1 + myGridBehavior.GetRaisingOffset();
            mySprite.SetPosition(new Vector2(myPosition.X , yPosition) * aTileSize + myGridBehavior.GetOffset() + new Vector2(-1, -1));
            mySprite.Draw(aSpriteBatch);
        }
    }
}
