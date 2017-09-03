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

    class FallingBlock : AbstractColorBlock
    {
        private const float BaseSpeed = 0.25f;
        protected float myYOffset = 0.0f;

        public FallingBlock(EBlockColor aColor)
        {
            myColor = aColor;
        }

        public override void Update()
        {
            myYOffset -= BaseSpeed;
        }

        public bool WillPassTile()
        {
            return myYOffset - BaseSpeed < 0.0f;
        }

        public void PassTile()
        {
            myGridArea.Y--;
            myYOffset += 1.0f;
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            Vector2 position = GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset);
            position.Y -= myYOffset * mySprite.GetSize().Y;
            mySprite.SetPosition(position);
            mySprite.Draw(aSpriteBatch);
        }
    }
}
