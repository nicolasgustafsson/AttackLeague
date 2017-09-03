using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AttackLeague.AttackLeague
{
    class Player
    {
        private Sprite mySprite;
        private Point myPosition = new Point(2,5);
        private Grid myGrid;

        public Player(ContentManager aContent, Grid aGrid)
        {
            BindActions();

            myGrid = aGrid;
            mySprite = new Sprite("PlayerMarker", aContent);
        }

        protected virtual void BindActions()
        {
            ActionMapper.BindAction("MoveLeft", Keys.A, KeyStatus.KeyPressed);
            ActionMapper.BindAction("MoveLeft", Keys.Left, KeyStatus.KeyPressed);

            ActionMapper.BindAction("MoveRight", Keys.D, KeyStatus.KeyPressed);
            ActionMapper.BindAction("MoveRight", Keys.Right, KeyStatus.KeyPressed);

            ActionMapper.BindAction("MoveUp", Keys.W, KeyStatus.KeyPressed);
            ActionMapper.BindAction("MoveUp", Keys.Up, KeyStatus.KeyPressed);

            ActionMapper.BindAction("MoveDown", Keys.S, KeyStatus.KeyPressed);
            ActionMapper.BindAction("MoveDown", Keys.Down, KeyStatus.KeyPressed);

            ActionMapper.BindAction("SwapBlocks", Keys.E, KeyStatus.KeyPressed);

            ActionMapper.BindAction("RaiseBlocks", Keys.Space, KeyStatus.KeyDown);
        }

        public void Update()
        {
            if (myGrid.HasRaisedGridThisFrame())
                myPosition.Y++;
            HandleMovement();

            if (ActionMapper.ActionIsActive("SwapBlocks"))
                myGrid.SwapRight(myPosition);

            if (ActionMapper.ActionIsActive("RaiseBlocks"))
                myGrid.SetIsRaisingBlocks();
        }

        private void HandleMovement()
        {
            if (ActionMapper.ActionIsActive("MoveRight"))
            { 
                if (myPosition.X < myGrid.GetWidth() - 2)
                {
                    myPosition.X += 1;
                }
            }
            if (ActionMapper.ActionIsActive("MoveLeft"))
            {
                if (myPosition.X > 0)
                {
                    myPosition.X -= 1;
                }
            }
            if (ActionMapper.ActionIsActive("MoveUp"))
            {
                if (myPosition.Y < myGrid.GetHeight() - 1)
                {
                    myPosition.Y += 1;
                }
            }
            if (ActionMapper.ActionIsActive("MoveDown"))
            {
                if (myPosition.Y > 1)
                {
                    myPosition.Y -= 1;
                }
            }
        }

        public void Draw(SpriteBatch aSpriteBatch, float aTileSize)
        {
            float invertedYPosition = myGrid.GetHeight() - myPosition.Y;
            float yPosition = invertedYPosition - 1 + myGrid.GetRaisingOffset();
            mySprite.SetPosition(new Vector2(myPosition.X , yPosition) * aTileSize + myGrid.GetOffset());
            mySprite.Draw(aSpriteBatch);
        }
    }
}
