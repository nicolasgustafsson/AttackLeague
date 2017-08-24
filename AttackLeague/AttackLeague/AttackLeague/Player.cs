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
        private Point myPosition = new Point(0,0);
        private Grid myGrid;

        public Player(ContentManager aContent, Grid aGrid)
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

            ActionMapper.BindAction("RaiseBlocks", Keys.Space, KeyStatus.KeyPressed);

            myGrid = aGrid;
            mySprite = new Sprite("PlayerMarker", aContent);
        }

        public void Update()
        {
            HandleMovement();

            if (ActionMapper.ActionIsActive("SwapBlocks"))
            {
                myGrid.SwapRight(myPosition);
            }

            if (ActionMapper.ActionIsActive("RaiseBlocks"))
            {
                myGrid.RaiseBlocks();
            }
        }

        private void HandleMovement()
        {
            if (ActionMapper.ActionIsActive("MoveRight"))
            { 
                if (myPosition.X < 4)
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
                if (myPosition.Y < 11)
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
            mySprite.SetPosition(new Vector2(myPosition.X , myGrid.GetHeight() - myPosition.Y -1) * aTileSize + myGrid.GetOffset());
            mySprite.Draw(aSpriteBatch);
        }
    }
}
