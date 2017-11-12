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

    class DisappearingBlock : AbstractColorBlock
    {
        private float myCurrentFrame = 0;
        private int myStartFrame;
        private int myTotalFrames;

        public DisappearingBlock(EBlockColor aColor, int aTotalFrames, int aStartFrame)
        {
            myTotalFrames = aTotalFrames;
            myStartFrame = aStartFrame;
            myColor = aColor;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            //make block darky
            Color color = GetColorFromEnum() * 0.3f;
            color.A = 255;
            mySprite.SetColor(color);
        }

        public override void Update(float aGameSpeed)
        {
            myCurrentFrame += 1f + (aGameSpeed / 3.0f);
        }

        private float GetAnimationProgress()
        {
            float animationLength = 30;
            float currentFrameInAnimation = (int)myCurrentFrame - myStartFrame;
            return Math.Max(0f, Math.Min(currentFrameInAnimation / animationLength, 1.0f));
        }

        public bool IsAlive()
        {
            return (int)myCurrentFrame < myTotalFrames;
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            mySprite.SetScale(Vector2.One - Vector2.One * GetAnimationProgress());
            mySprite.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset));
            mySprite.Draw(aSpriteBatch);

            if (myIcon == null)
                return;

            myIcon.SetScale(Vector2.One - Vector2.One * GetAnimationProgress());
            myIcon.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset));
            myIcon.Draw(aSpriteBatch);
        }
    }
}
