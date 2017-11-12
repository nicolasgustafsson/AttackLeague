using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague
{
    enum EBlockColor
    {                                     //blixt, måne, sol, tornado, moln, regndroppe, snöflinga stjärna
        Cyan,      //Snöflinga
        Magenta,   //Måne
        Yellow,    //Blixt
        Red,       //Sol
        Green,     //Stjärna
        Blue,      //Regndroppe
        Grey,      //Tornado
        None
    }

    abstract class AbstractColorBlock : AbstractBlock
    {
        protected Sprite mySprite;
        protected Sprite myIcon;
        protected EBlockColor myColor = EBlockColor.None;

        public AbstractColorBlock()
        {
            if(myColor == EBlockColor.None)
            {
                RandomizeColor();
            }


        }

        public virtual void RandomizeColor()
        {
            myColor = (EBlockColor)Randomizer.GlobalRandomizer.Next(0, 5);
            if (mySprite != null)
            {
                mySprite.SetColor(GetColorFromEnum());
            }
        }

        public override void LoadContent()
        {
            mySprite = new Sprite("tiley");
            mySprite.SetColor(GetColorFromEnum());
            myIcon = GetIconFromEnum();
        }

        public Color GetColorFromEnum()
        {
            switch (myColor)
            {
                case EBlockColor.Cyan:
                    return new Color(0, 255, 255);
                case EBlockColor.Magenta:
                    return new Color(186, 0, 186);
                case EBlockColor.Yellow:
                    return Color.Yellow;
                case EBlockColor.Red:
                    return new Color(215, 0, 0, 255);
                case EBlockColor.Green:
                    return Color.LimeGreen;
                case EBlockColor.Blue:
                    return Color.DarkBlue;
                case EBlockColor.Grey:
                    return Color.HotPink;
                default:
                    return Color.White;
            }
        }

        public Sprite GetIconFromEnum()
        {
            switch (myColor)
            {
                case EBlockColor.Cyan:
                    return new Sprite("SnowStar");
                case EBlockColor.Magenta:
                    return new Sprite("Moonie");
                case EBlockColor.Yellow:
                    return new Sprite("LightningThunderLoudThingie");
                case EBlockColor.Red:
                    return new Sprite("Sunnie");
                case EBlockColor.Green:
                    return new Sprite("Star");
                case EBlockColor.Blue:
                    return new Sprite("Droppy");
                case EBlockColor.Grey:
                    return new Sprite("Strom");
                default:
                    return null;
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset) 
        {
            mySprite.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset));
            mySprite.Draw(aSpriteBatch);

            if (myIcon == null)
                return;

            myIcon.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset));
            myIcon.Draw(aSpriteBatch);
        }

        public override int GetHeight()
        {
            return myGridArea.Bottom;
        }

        public override int GetXCoordinate()
        {
            return myGridArea.Right;
        }

        public EBlockColor GetColor()
        {
            return myColor;
        }

        public Rectangle GetRectangle()
        {
            return myGridArea;
        }
    }
}
