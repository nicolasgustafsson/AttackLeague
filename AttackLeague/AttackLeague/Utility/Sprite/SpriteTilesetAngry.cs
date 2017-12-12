using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.Sprite
{
    class SpriteTilesetAngry : SpriteTileset
    {
        // Angry sprite tiles are structured like this
        //[_][_ _ _]
        //[ ][     ]      1
        //[ ][     ]   4     2
        //[_][_ _ _]      3

        public SpriteTilesetAngry()
            :base("angrytemplate")
        {
            myRectangle.Width = myTexture.Width / 4;
            myRectangle.Height = myTexture.Height / 4;
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, myRectangle, myColor, 0f, Vector2.Zero, myScale, SpriteEffects.None, 0);
        }

        // set manually below --------------

        public void SetManually(bool aHasBuddyAbove, bool aHasBuddyRight, bool aHasBuddyDown, bool aHasBuddyLeft)
        {
            int total = 0;
            total += aHasBuddyAbove ? 1 : 0;
            total += aHasBuddyRight ? 2 : 0;
            total += aHasBuddyDown ? 4 : 0;
            total += aHasBuddyLeft ? 8 : 0;

            switch(total)
            {
                case 0:
                    SetToAlone();
                    break;
                case 1:
                    SetToVerticalBottom();
                    break;
                case 2:
                    SetToHorizontalLeft();
                    break;
                case 3:
                    SetToBottomLeft();
                    break;
                case 4:
                    SetToVerticalTop();
                    break;
                case 5:
                    SetToVerticalMiddle();
                    break;
                case 6:
                    SetToTopLeft();
                    break;
                case 7:
                    SetToMiddleLeft();
                    break;
                case 8:
                    SetToHorizontalRight();
                    break;
                case 9:
                    SetToBottomRight();
                    break;
                case 10:
                    SetToHorizontalMiddle();
                    break;
                case 11:
                    SetToBottomMiddle();
                    break;
                case 12:
                    SetToTopRight();
                    break;
                case 13:
                    SetToMiddleRight();
                    break;
                case 14:
                    SetToTopMiddle();
                    break;
                case 15:
                    SetToMiddleMiddle();
                    break;
            }
        }

        public void SetToAlone()
        {
            myRectangle.X = 0;
            myRectangle.Y = 0;
        }

        public void SetToHorizontalLeft()
        {
            myRectangle.X = myRectangle.Width * 1;
            myRectangle.Y = myRectangle.Height * 0;
        }

        public void SetToHorizontalMiddle()
        {
            myRectangle.X = myRectangle.Width * 2;
            myRectangle.Y = myRectangle.Height * 0;
        }

        public void SetToHorizontalRight()
        {
            myRectangle.X = myRectangle.Width * 3;
            myRectangle.Y = myRectangle.Height * 0;
        }

        public void SetToVerticalTop()
        {
            myRectangle.X = myRectangle.Width * 0;
            myRectangle.Y = myRectangle.Height * 1;
        }

        public void SetToTopLeft()
        {
            myRectangle.X = myRectangle.Width * 1;
            myRectangle.Y = myRectangle.Height * 1;
        }

        public void SetToTopMiddle()
        {
            myRectangle.X = myRectangle.Width * 2;
            myRectangle.Y = myRectangle.Height * 1;
        }

        public void SetToTopRight()
        {
            myRectangle.X = myRectangle.Width * 3;
            myRectangle.Y = myRectangle.Height * 1;
        }

        public void SetToVerticalMiddle()
        {
            myRectangle.X = myRectangle.Width * 0;
            myRectangle.Y = myRectangle.Height * 2;
        }

        public void SetToMiddleLeft()
        {
            myRectangle.X = myRectangle.Width * 1;
            myRectangle.Y = myRectangle.Height * 2;
        }

        public void SetToMiddleMiddle()
        {
            myRectangle.X = myRectangle.Width * 2;
            myRectangle.Y = myRectangle.Height * 2;
        }

        public void SetToMiddleRight()
        {
            myRectangle.X = myRectangle.Width * 3;
            myRectangle.Y = myRectangle.Height * 2;
        }

        public void SetToVerticalBottom()
        {
            myRectangle.X = myRectangle.Width * 0;
            myRectangle.Y = myRectangle.Height * 3;
        }

        public void SetToBottomLeft()
        {
            myRectangle.X = myRectangle.Width * 1;
            myRectangle.Y = myRectangle.Height * 3;
        }

        public void SetToBottomMiddle()
        {
            myRectangle.X = myRectangle.Width * 2;
            myRectangle.Y = myRectangle.Height * 3;
        }

        public void SetToBottomRight()
        {
            myRectangle.X = myRectangle.Width * 3;
            myRectangle.Y = myRectangle.Height * 3;
        }
    }
}
