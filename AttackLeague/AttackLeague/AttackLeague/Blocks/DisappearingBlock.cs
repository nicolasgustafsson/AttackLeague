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
using AttackLeague.AttackLeague.Grid;

namespace AttackLeague.AttackLeague
{

    class DisappearingBlock : AbstractColorBlock
    {
        private float myCurrentFrame = 0;
        private int myStartFrame;
        private int myTotalFrames;

        public DisappearingBlock(GridBundle aGridBundle, EBlockColor aColor, int aTotalFrames, int aStartFrame)
            :base(aGridBundle)
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
            base.Update(aGameSpeed);

            if (IsAlive() == false)
            {
                Point position = GetPosition();
                myGridBundle.Generator.EliminateBlock(position);
                myGridBundle.Behavior.OnBlockEliminated(position);
            }

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

            myIcon.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset));

            //SetIconAnimation();
            myIcon.SetScale(Vector2.One - Vector2.One * GetAnimationProgress());
            myIcon.Draw(aSpriteBatch);
        }
    }
}
