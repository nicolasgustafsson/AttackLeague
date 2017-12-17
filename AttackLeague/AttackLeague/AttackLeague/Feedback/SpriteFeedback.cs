using AttackLeague.Utility.Betweenxt;
using AttackLeague.Utility.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague.Feedback
{
    class SpriteFeedback : IFeedback
    {
        public delegate bool DoneFunction();

        private Sprite mySprite;
        private Vector2 myPosition;
        private Color myColor;
        private Betweenxt myAlphaTween;
        private Betweenxt myPositionTween;
        private DoneFunction myDoneChecker;

        public SpriteFeedback(string aSprite, Vector2 aPosition, DoneFunction aDoneFunction, Betweenxt aAlphaTween = null, Betweenxt aPositionTween = null)
        {
            myAlphaTween = aAlphaTween;
            myPositionTween = aPositionTween;
            myPosition = aPosition;
            mySprite = new Sprite(aSprite);
            myColor = Color.White;
            myDoneChecker = aDoneFunction;
        }

        public void SetColor(Color aColor)
        {
            myColor = aColor;
        }

        public void Update()
        {
            if (myAlphaTween != null)
            {
                myAlphaTween.Update(1);
                myColor.A = (byte)myAlphaTween.GetValue();
            }
            if (myPositionTween != null)
            {
                myPositionTween.Update(1);
                myPosition.Y = myPositionTween.GetValue();
            }
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            mySprite.SetColor(myColor);
            mySprite.SetPosition(myPosition);
            mySprite.Draw(aSpriteBatch);
        }

        public bool IsDone()
        {
            return myDoneChecker();
        }
    }
}
