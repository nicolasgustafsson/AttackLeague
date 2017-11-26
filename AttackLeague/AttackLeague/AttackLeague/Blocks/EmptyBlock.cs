using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AttackLeague.AttackLeague
{

    public class EmptyBlock : AbstractBlock
    {
        private float mySwitchTimer = 0.0f;

        public EmptyBlock()
        {
            myGridArea = new Rectangle(0, 0, 1, 1);
        }

        public override void LoadContent()
        {
        }

        public override int GetHeight()
        {
            return myGridArea.Bottom;
        }

        public override int GetXCoordinate()
        {
            return myGridArea.Right;
        }

        public override void Update(float aGameSpeed)
        {
            mySwitchTimer -= 0.15f + 0.15f * ((aGameSpeed) * 0.1f);
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
        }

        public override void DoTheSwitchingCalculating(float aSwitchTime, ESwitchDirection aSwitchDirection)
        {
            mySwitchTimer = aSwitchTime;
        }

        public override bool IsSwitching()
        {
            return mySwitchTimer > 0.0f;
        }

    }
}
