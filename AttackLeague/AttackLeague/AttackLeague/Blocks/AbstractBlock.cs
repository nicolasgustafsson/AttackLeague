﻿using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace AttackLeague.AttackLeague
{
    public abstract class AbstractBlock : IComparable
    {
        protected Sprite mySprite;
        public Rectangle myGridArea = new Rectangle(0, 0, 1, 1);

        public virtual void LoadContent(ContentManager aContent)
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

        public void SetPosition(int aX, int aY)
        {
            myGridArea.X = aX;
            myGridArea.Y = aY;
        }

        public Point GetPosition()
        {
            return new Point(myGridArea.X, myGridArea.Y);
        }

        public abstract int GetHeight();
        public abstract int GetXCoordinate();

        public abstract void Update();

        public abstract void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight);


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
    }
}