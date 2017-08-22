using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague
{
    public abstract class AbstractBlock
    {
        private Sprite mySprite;


        public static bool operator <(AbstractBlock aFirst, AbstractBlock aSecond)
        {
            return aFirst.GetHeight() < aSecond.GetHeight();
        }

        public static bool operator >(AbstractBlock aFirst, AbstractBlock aSecond)
        {
            return aFirst.GetHeight() > aSecond.GetHeight();
        }

        protected abstract int GetHeight();

        protected abstract void Update(float aDeltaTime);

        protected abstract void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, Vector2 aTilePosition);


    }
}
