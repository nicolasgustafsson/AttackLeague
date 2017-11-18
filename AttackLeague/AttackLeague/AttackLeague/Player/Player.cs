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
        protected GameGrid myGrid;
        protected bool myIsPaused = false;
        public PlayerInfo myPlayerInfo;

        public Player(PlayerInfo aInfo)
        {
            myPlayerInfo = aInfo;

            myGrid = new GameGrid(myPlayerInfo.myPlayerIndex);
            mySprite = new Sprite("curse");
        }

        public virtual void Update()
        {
            HandleMovement();

            if (myIsPaused == false)
                myGrid.Update();

            if (myPlayerInfo.myMappedActions.ActionIsActive("SwapBlocks"))
                myGrid.SwapBlocksRight(myPosition);

            if (myPlayerInfo.myMappedActions.ActionIsActive("RaiseBlocks"))
                myGrid.SetIsRaisingBlocks();
        }

        private void HandleMovement()
        {
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveRight"))
            { 
                if (myPosition.X < myGrid.GetWidth() - 2)
                    myPosition.X += 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveLeft"))
            {
                if (myPosition.X > 0)
                    myPosition.X -= 1;
            }
            if (myPlayerInfo.myMappedActions.ActionIsActive("MoveUp"))
            {
                if (myPosition.Y < myGrid.GetHeight())
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
            if (myGrid.HasRaisedGridThisFrame())
                myPosition.Y++;

            myGrid.Draw(aSpriteBatch);

            float invertedYPosition = myGrid.GetHeight() - myPosition.Y;
            float yPosition = invertedYPosition - 1 + myGrid.GetRaisingOffset();
            mySprite.SetPosition(new Vector2(myPosition.X , yPosition) * aTileSize + myGrid.GetOffset() + new Vector2(-1, -1));
            mySprite.Draw(aSpriteBatch);
        }
    }
}
