using AttackLeague.Utility.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackLeague.Utility.GUI
{
    class TextBox : Button
    {
        bool myHasFocus = false;
        public string myText = "";

        public TextBox()
        {
            OnClicked += GainFocus;
            OnLostFocus += LoseFocus;
            SetSprite("pixel", new Point(512, 512), new Point(310, 150), true);
            // set scale to some size?
            // set position to some thing
            // set my rext to pos and scale
            SetSpriteColor(Color.Gray);
        }

        bool GainFocus()
        {
            myHasFocus = true;
            SetSpriteColor(Color.Green);
            return true;
        }

        bool LoseFocus()
        {
            myHasFocus = false;
            SetSpriteColor(Color.Gray);
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
            // draw my text as text
        }
    }
}
