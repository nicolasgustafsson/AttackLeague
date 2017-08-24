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

    class DisappearingBlock : AbstractBlock
    {
        private Color myColor;
        private int myCurrentFrame = 0;
        private int myStartFrame;
        private int myTotalFrames;

        public DisappearingBlock(Color aColor, int aTotalFrames, int aStartFrame)
        {
            myTotalFrames = aTotalFrames;
            myStartFrame = aStartFrame;

            myColor = aColor * 0.3f;
            myColor.A = 255;
            myGridArea = new Rectangle(0, 0, 1, 1);
        }

        public override void LoadContent(ContentManager aContent)
        {
            mySprite = new Sprite("ColorBlock", aContent);
            mySprite.SetColor(myColor);
        }

        public override int GetHeight()
        {
            return myGridArea.Bottom;
        }

        public override int GetXCoordinate()
        {
            return myGridArea.Right;
        }

        public override void Update()
        {
            myCurrentFrame++;
        }

        private float GetAnimationProgress()
        {
            float animationLength = 30;
            float currentFrameInAnimation = myCurrentFrame - myStartFrame;
            return Math.Max(0f, Math.Min(currentFrameInAnimation / animationLength, 1.0f));
        }

        public bool IsAlive()
        {
            return myCurrentFrame < myTotalFrames;
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            mySprite.SetScale(Vector2.One - Vector2.One * GetAnimationProgress());
            mySprite.SetPosition(aGridOffset + new Vector2(myGridArea.X * mySprite.GetSize().X, aRaisingOffset + (aGridHeight - myGridArea.Y) * mySprite.GetSize().Y));
            mySprite.Draw(aSpriteBatch);
        }
    }
}
