﻿using System;
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
using AttackLeague.AttackLeague.Grid;

namespace AttackLeague.AttackLeague
{
    class ColorBlock : AbstractColorBlock
    {
        private bool myCanChain = true;
        private Betweenxt myGroovyDanceMoves = null;
        private float myDanceOffset = 0.0f;
        private ESwitchDirection myDancingDirection = ESwitchDirection.Nope;

        public ColorBlock(GridBundle aGridBundle)
            :base(aGridBundle)
        {
        }

        public ColorBlock(GridBundle aGridBundle, EBlockColor aColor)
            :base(aGridBundle)
        {
            myColor = aColor;
        }

        public ColorBlock(GridBundle aGridBundle, FallingBlock aBlock)
            :base(aGridBundle)
        {
            myColor = aBlock.GetColor();
        }

        public override void Update(float aGameSpeed)
        {
            base.Update(aGameSpeed);
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

            if (IsSwitching())
                return;

            if (CanFall())
            {
                Point position = GetPosition();
                myGridBundle.Container.InitializeBlock(position, new FallingBlock(myGridBundle, this));
            }
        }

        private bool CanFall()
        {
            Rectangle rectangleCopy = GetRectangle();

            if (rectangleCopy.Y != 0)
            {
                rectangleCopy.Y--;
                return myGridBundle.Behavior.RectangleIntersectsForFallingPurposes(rectangleCopy) == false;
            }

            return false;
        }

        public override void DoTheSwitchingCalculating(float aSwitchTime, ESwitchDirection aSwitchDirection)
        {
            myGroovyDanceMoves = new Betweenxt(Betweenxt.Lerp, 1.0f, 0.0f, 0.0f, aSwitchTime);
            myDancingDirection = aSwitchDirection;
            myDanceOffset = 0.0f;
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            float danceDirection = 0.0f;
            if (myDancingDirection == ESwitchDirection.ToTheLeft)
            {
                danceDirection = 48.0f;
            }
            else if (myDancingDirection == ESwitchDirection.ToTheRight)
            {
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
