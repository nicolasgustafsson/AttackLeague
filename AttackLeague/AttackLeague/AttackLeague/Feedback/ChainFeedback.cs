using AttackLeague.Utility;
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
    public class ChainComboFeedback : IFeedback
    {
        private Sprite myBackdropSprite;
        private SpriteFont myFont;
        private string myText;
        private Vector2 myPosition;
        private Color myBackdropColor;
        private Betweenxt myAlphaTween;
        private Betweenxt myPositionTween;

        public ChainComboFeedback(string aPrefix, int aChainNumber, Vector2 aPosition)
        {
            myPosition = aPosition;
            myText = aPrefix + aChainNumber.ToString(); 
            myBackdropSprite = new Sprite("chainBackdrop");
            myFont = ContentManagerInstance.Content.Load<SpriteFont>("raditascartoon");
            myBackdropColor = GetColorFromChain(aChainNumber - 2);

            myAlphaTween = new Betweenxt(Betweenxt.Lerp, 255, 0, 0, 60);
            myPositionTween = new Betweenxt(Betweenxt.Lerp, aPosition.Y, aPosition.Y - 60, 0, 60);
        }

        private Color GetColorFromChain(int aStep)
        {
            int hue = 20 * aStep;
            return ColorUtility.FromHSV(hue, 1, 1);
        }

        public void Update()
        {
            myAlphaTween.Update(1);
            myPositionTween.Update(1);

            myPosition.Y = myPositionTween.GetValue();
            //myBackdropColor.A = (byte)myAlphaTween.GetValue();
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            myBackdropSprite.SetColor(myBackdropColor);
            myBackdropSprite.SetPosition(myPosition);

            myBackdropSprite.Draw(aSpriteBatch);
            aSpriteBatch.DrawString(myFont, myText, myPosition, Color.White);

            //aSpriteBatch.DrawString(myFont, 
            //    "Bonus time: " + myChainTimer.ToString() + 
            //    "\nGame Time: " + myGameTime.ToString() + 
            //    "\nGame Speed: " + myGameSpeed.ToString()
            //    , new Vector2(500, 100), Color.MidnightBlue);
        }

        public bool IsDone()
        {
            return myAlphaTween.IsFinished();
        }
    }
}
