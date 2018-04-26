using AttackLeague.Utility.Input;
using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AttackLeague.Utility.GUI
{
    class TextBox : Button
    {
        bool myHasFocus = false;
        public string myText = "";
        public delegate void TextBoxEvent(TextBox sender);
        public TextBoxEvent OnEnterPressed;
        public TextBoxEvent OnTabPressed;

        public SpriteFont myFont;

        public TextBox()
        {
            OnClicked += GainFocus;
            OnLostFocus += LoseFocus;

            myFont = ContentManagerInstance.Content.Load<SpriteFont>("raditascartoon");
            myFont.DefaultCharacter = '?';

            SetSprite("pixel", new Point(512, 512), new Point(310, 150), true);
            // set scale to some size?
            // set position to some thing
            // set my rext to pos and scale
            SetSpriteColor(Color.Gray);
        }

        void gotabokstav(object aSender, CharacterEventArgs argies)
        {
            switch(argies.Character)
            {
                 case '\b': //backspace
                    if (myText.Length > 0)
                        myText = myText.Substring(0, myText.Length - 1);
                    break;
                case '\r': //return
                        OnEnterPressed?.Invoke(this);
                    break;
                case '\t': //tab
                        OnTabPressed?.Invoke(this);
                    break;
                default:
                    myText += argies.Character;
                    break;
            }
        }

        bool GainFocus()
        {
            if (myHasFocus)
                return true;

            myHasFocus = true;
            SetSpriteColor(Color.Green);
            EventInput.CharEntered += gotabokstav;
            return true;
        }

        bool LoseFocus()
        {
            if (!myHasFocus)
                return true;

            myHasFocus = false;
            SetSpriteColor(Color.Gray);
            EventInput.CharEntered -= gotabokstav;
            return true;
        }

        public override void Update()
        {
            base.Update();
            if (myHasFocus)
            {

                // acquire input to string thingy

            }
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            base.Draw(aSpriteBatch);
            aSpriteBatch.DrawString(myFont, myText, mySprite.GetPosition(), Color.White);
            // draw my text as text
        }
    }
}
