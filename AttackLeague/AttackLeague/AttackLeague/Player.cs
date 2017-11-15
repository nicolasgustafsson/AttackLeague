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

namespace AttackLeague.AttackLeague
{
    class Player
    {
        private Sprite mySprite;
        private Point myPosition = new Point(2,5);
        private GameGrid myGrid;

        public Player( GameGrid aGrid)
        {
            BindActions();

            myGrid = aGrid;
            mySprite = new Sprite("curse");
        }

        protected virtual void BindActions()
        {
            ActionMapper.BindAction("MoveLeft", Keys.A, InputStatus.KeyCooldown);
            ActionMapper.BindAction("MoveLeft", Keys.Left, InputStatus.KeyCooldown);

            ActionMapper.BindAction("MoveRight", Keys.D, InputStatus.KeyCooldown);
            ActionMapper.BindAction("MoveRight", Keys.Right, InputStatus.KeyCooldown);

            ActionMapper.BindAction("MoveUp", Keys.W, InputStatus.KeyCooldown);
            ActionMapper.BindAction("MoveUp", Keys.Up, InputStatus.KeyCooldown);

            ActionMapper.BindAction("MoveDown", Keys.S, InputStatus.KeyCooldown);
            ActionMapper.BindAction("MoveDown", Keys.Down, InputStatus.KeyCooldown);

            ActionMapper.BindAction("SwapBlocks", Keys.E, InputStatus.KeyPressed);

            ActionMapper.BindAction("RaiseBlocks", Keys.Space, InputStatus.KeyDown);

            //--

            ActionMapper.BindAction("MoveLeft", Buttons.DPadLeft, InputStatus.KeyCooldown, 0);
            ActionMapper.BindAction("MoveLeft", Buttons.LeftThumbstickLeft, InputStatus.KeyCooldown, 0);

            ActionMapper.BindAction("MoveRight", Buttons.DPadRight, InputStatus.KeyCooldown, 0);
            ActionMapper.BindAction("MoveRight", Buttons.LeftThumbstickRight, InputStatus.KeyCooldown, 0);

            ActionMapper.BindAction("MoveUp", Buttons.DPadUp, InputStatus.KeyCooldown, 0);
            ActionMapper.BindAction("MoveUp", Buttons.LeftThumbstickUp, InputStatus.KeyCooldown, 0);

            ActionMapper.BindAction("MoveDown", Buttons.DPadDown, InputStatus.KeyCooldown, 0);
            ActionMapper.BindAction("MoveDown", Buttons.LeftThumbstickDown, InputStatus.KeyCooldown, 0);

            ActionMapper.BindAction("SwapBlocks", Buttons.A, InputStatus.KeyPressed, 0);
            ActionMapper.BindAction("SwapBlocks", Buttons.B, InputStatus.KeyPressed, 0);

            ActionMapper.BindAction("RaiseBlocks", Buttons.RightShoulder, InputStatus.KeyDown, 0);
            ActionMapper.BindAction("RaiseBlocks", Buttons.RightTrigger, InputStatus.KeyDown, 0);

            ActionMapper.BindAction("RaiseBlocks", Buttons.LeftShoulder, InputStatus.KeyDown, 0);
            ActionMapper.BindAction("RaiseBlocks", Buttons.LeftTrigger, InputStatus.KeyDown, 0);
        }

        public void Update()
        {
            if (myGrid.HasRaisedGridThisFrame())
                myPosition.Y++;
            HandleMovement();

            if (ActionMapper.ActionIsActive("SwapBlocks"))
                myGrid.SwapBlocksRight(myPosition);

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
                if (myPosition.Y < myGrid.GetHeight())
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
            mySprite.SetPosition(new Vector2(myPosition.X , yPosition) * aTileSize + myGrid.GetOffset() + new Vector2(-1, -1));
            mySprite.Draw(aSpriteBatch);
        }
    }
}
