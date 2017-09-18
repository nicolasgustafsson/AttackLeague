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
        private const float MyBaseSpeed = 0.15f;
        private const float MyAdditionalSpeed = 0.1f;
        private bool myCanChain = false;
        protected float myYOffset = 0.0f;

        public FallingBlock(EBlockColor aColor)
        {
            myColor = aColor;
        }

        public FallingBlock(ColorBlock aBlock)
        {
            myColor = aBlock.GetColor();
            myCanChain = aBlock.CanChain;
        }

        public override void Update(float aGameSpeed)
        {
            myYOffset -= GetFallingSpeed(aGameSpeed);
        }

        public bool WillPassTile(float aGameSpeed)
        {
            return (myYOffset - GetFallingSpeed(aGameSpeed)) < 0.0f;
        }

        private static float GetFallingSpeed(float aGameSpeed)
        {
            return MyBaseSpeed + 0.75f * ((aGameSpeed -1f) * 0.1f); // todo un-hardcode if needed
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

        public bool CanChain
        {
            get { return myCanChain; }
            set { myCanChain = value; }
        }
    }
}
