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
            myGrid = aGrid;
            mySprite = new Sprite("PlayerMarker", aContent);
        }

        public void Update()
        {
            HandleMovement();

            if (KeyboardWrapper.KeyPressed(Keys.E))
            {
                myGrid.SwapRight(myPosition);
            }

            if (KeyboardWrapper.KeyPressed(Keys.Space))
            {
                myGrid.RaiseBlocks();
            }
        }

        private void HandleMovement()
        {
            if (KeyboardWrapper.KeyPressed(Keys.D) || KeyboardWrapper.KeyPressed(Keys.Right))
            { 
                if (myPosition.X < 4)
                {
                    myPosition.X += 1;
                }
            }
            if (KeyboardWrapper.KeyPressed(Keys.A) || KeyboardWrapper.KeyPressed(Keys.Left))
            {
                if (myPosition.X > 0)
                {
                    myPosition.X -= 1;
                }
            }
            if (KeyboardWrapper.KeyPressed(Keys.W) || KeyboardWrapper.KeyPressed(Keys.Up))
            {
                if (myPosition.Y < 11)
                {
                    myPosition.Y += 1;
                }
            }
            if (KeyboardWrapper.KeyPressed(Keys.S) || KeyboardWrapper.KeyPressed(Keys.Down))
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
