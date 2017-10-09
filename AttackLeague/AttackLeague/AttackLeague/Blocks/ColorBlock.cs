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
        private bool myWannaDance = true;

        public ColorBlock()
        {
            myGroovyDanceMoves = new Betweenxt(Betweenxt.Lerp, 0.0f, 1.0f); 
        }

        public ColorBlock(EBlockColor aColor)
        {
            myColor = aColor;
            myGroovyDanceMoves = new Betweenxt(Betweenxt.Lerp, 0.0f, 1.0f);
        }

        public ColorBlock(FallingBlock aBlock)
        {
            myColor = aBlock.GetColor();
            CanChain = aBlock.CanChain;
            myGroovyDanceMoves = new Betweenxt(Betweenxt.Lerp, 0.0f, 1.0f);
        }

        public override void Update(float aGameSpeed)
        {
            if (myWannaDance == true)
            {
                myGroovyDanceMoves.Update(aGameSpeed);
                myDanceOffset = myGroovyDanceMoves.GetProgress();
                if (myGroovyDanceMoves.IsFinished())
                    myWannaDance = false;
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            mySprite.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset)); // + new Vector2(myDanceOffset * 48.0f, 0.0f)
            mySprite.Draw(aSpriteBatch);
        }

        public bool CanChain
        {
            get { return myCanChain; }
            set { myCanChain = value; }
        }
    }
}
