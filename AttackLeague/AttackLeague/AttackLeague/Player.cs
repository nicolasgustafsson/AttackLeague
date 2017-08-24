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
        private bool myPressedRight = false;
        private bool myPressedLeft = false;
        private bool myPressedUp = false;
        private bool myPressedDown = false;
        private bool myPressedSwitch = false;

        public Player(ContentManager aContent, Grid aGrid)
        {
            myGrid = aGrid;
            mySprite = new Sprite("PlayerMarker", aContent);
        }

        public void Update()
        {
            HandleMovement();

            if (myPressedSwitch == false && Keyboard.GetState().IsKeyDown(Keys.E) )
            {
                myPressedSwitch = true;
                myGrid.SwapRight(myPosition);
            }
            if (Keyboard.GetState().IsKeyUp(Keys.E))
            {
                myPressedSwitch = false;
            }
        }

        private void HandleMovement()
        {
            if (myPressedRight == false && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                myPressedRight = true;
                if (myPosition.X < 4)
                {
                    myPosition.X += 1;
                }
            }
            if (myPressedLeft == false && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                myPressedLeft = true;
                if (myPosition.X > 0)
                {
                    myPosition.X -= 1;
                }
            }
            if (myPressedUp == false && Keyboard.GetState().IsKeyDown(Keys.W))
            {
                myPressedUp = true;
                if (myPosition.Y < 11)
                {
                    myPosition.Y += 1;
                }
            }
            if (myPressedDown == false && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                myPressedDown = true;
                if (myPosition.Y > 1)
                {
                    myPosition.Y -= 1;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.D))
            {
                myPressedRight = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.A))
            {
                myPressedLeft = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.W))
            {
                myPressedUp = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.S))
            {
                myPressedDown = false;
            }

        }

        public void Draw(SpriteBatch aSpriteBatch, float aTileSize)
        {
            mySprite.SetPosition(new Vector2(myPosition.X , myGrid.GetHeight() - myPosition.Y -1) * aTileSize + myGrid.GetOffset());
            mySprite.Draw(aSpriteBatch);
        }

    }
}
