using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using AttackLeague.AttackLeague.Grid;

namespace AttackLeague.AttackLeague
{
    public enum ESwitchDirection
    {
        ToTheLeft,
        ToTheRight,
        Nope
    }

    public abstract class AbstractBlock : IComparable
    {
        public Rectangle myGridArea = new Rectangle(0, 0, 1, 1);

        protected GridBundle myGridBundle;

        //protected float myRaisingOffset = 0.0f;

        public AbstractBlock(GridBundle aGridBundle)
        {
            myGridBundle = aGridBundle;
        }

        public virtual void LoadContent()
        {
        }

        public static bool operator <(AbstractBlock aFirst, AbstractBlock aSecond)
        {
            if (aFirst.GetHeight() == aSecond.GetHeight())
            {
                return aFirst.GetXCoordinate() < aSecond.GetXCoordinate();
            }
            return aFirst.GetHeight() < aSecond.GetHeight();
        }

        public static bool operator >(AbstractBlock aFirst, AbstractBlock aSecond)
        {
            if (aFirst.GetHeight() == aSecond.GetHeight())
            {
                return aFirst.GetXCoordinate() > aSecond.GetXCoordinate();
            }
            return aFirst.GetHeight() > aSecond.GetHeight();
        }

        public void SetPosition(Point aPosition)
        {
            SetPosition(aPosition.X, aPosition.Y);
        }

        public int GetPositionWorth()
        {
            Point position = GetPosition();
            return position.Y * myGridBundle.Container.GetInitialWidth() + position.X;
        }

        public void SetPosition(int aX, int aY)
        {
            myGridArea.X = aX;
            myGridArea.Y = aY;
        }

        public Point GetPosition()
        {
            return new Point(myGridArea.X, myGridArea.Y);
        }

        public virtual void DoTheSwitchingCalculating(float aSwitchTime, ESwitchDirection aSwitchDirection)
        {
            Debug.Assert(false, "Implement this function in subclass!");
        }

        public Vector2 GetScreenPosition(Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            float invertedGridYPosition = (aGridHeight - myGridArea.Y);
            float xPosition = myGridArea.X * GetTileSize();
            float yPosition = (aRaisingOffset + invertedGridYPosition) * GetTileSize();
            return aGridOffset + new Vector2(xPosition, yPosition);
        }

        public static float GetTileSize()
        {
            return 48f;
        }

        public abstract int GetHeight();
        public abstract int GetXCoordinate();

        public abstract void Update(float aGameSpeed);

        public virtual void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            //Intentionally left empty
        }
        public int CompareTo(object obj)
        {
            if (obj is AbstractBlock)
            {
                if (this > (AbstractBlock)obj)
                {
                    return 1;
                }
                if (this == (AbstractBlock) obj)
                {
                    return 0;
                }
                return -1;
            }
            return 0;
        }

        public virtual bool IsSwitching()
        {
            return false;
        }

        public virtual bool AllowsFalling()
        {
            return false;
        }

        protected float GetMagicalSpeed(float aGameSpeed)
        {
            //0.15 is the magic number
            return 0.15f + 0.15f * ((aGameSpeed) * 0.1f);
        }
    }
}
