using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;
using AttackLeague.Utility.Betweenxt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AttackLeague.AttackLeague
{


    class ColorBlock : AbstractColorBlock
    {
        private bool myCanChain = false;
        private Betweenxt myGroovyDanceMoves = null;
        private float myDanceOffset = 0.0f;
        private ESwitchDirection myDancingDirection = ESwitchDirection.Nope;

        public ColorBlock()
        {
        }

        public ColorBlock(EBlockColor aColor)
        {
            myColor = aColor;
        }

        public ColorBlock(FallingBlock aBlock)
        {
            myColor = aBlock.GetColor();
            CanChain = aBlock.CanChain;
        }

        public override void Update(float aGameSpeed)
        {
            if (myDancingDirection != ESwitchDirection.Nope)
            {
                myGroovyDanceMoves.Update(GetMagicalSpeed(aGameSpeed));
                myDanceOffset = myGroovyDanceMoves.GetValue();
                if (myGroovyDanceMoves.IsFinished())
                {
                    myGroovyDanceMoves.Reset();
                    myDancingDirection = ESwitchDirection.Nope;
                }
            }
        }

        public override void DoTheSwitchingCalculating(float aSwitchTime, ESwitchDirection aSwitchDirection)
        {
            myGroovyDanceMoves = new Betweenxt(Betweenxt.Lerp, 1.0f, 0.0f, 0.0f, aSwitchTime);
            myDancingDirection = aSwitchDirection;
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            float danceDirection = 0.0f;
            if (myDancingDirection == ESwitchDirection.ToTheLeft)
            {
                Console.WriteLine("Switchy To Left {0}, {1}", myColor.ToString(), myDanceOffset.ToString());
                danceDirection = 48.0f;
            }
            else if (myDancingDirection == ESwitchDirection.ToTheRight)
            {
                Console.WriteLine("Switchy To Right {0}, {1}", myColor.ToString(), myDanceOffset.ToString());
                danceDirection = -48.0f;
            }

            mySprite.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset) + new Vector2((1.0f - myDanceOffset) *  danceDirection, 0.0f)); //  + 
            mySprite.Draw(aSpriteBatch);

            if (myIcon == null)
                return;

            myIcon.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset) + new Vector2((1.0f - myDanceOffset) * danceDirection, 0.0f));
            SetIconAnimation();
            myIcon.Draw(aSpriteBatch);
        }

        public bool CanChain
        {
            get { return myCanChain; }
            set { myCanChain = value; }
        }

        public override bool IsSwitching()
        {
            return myDancingDirection != ESwitchDirection.Nope;
        }
    }
}
