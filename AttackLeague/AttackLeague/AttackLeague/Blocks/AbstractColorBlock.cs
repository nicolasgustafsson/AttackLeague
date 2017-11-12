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
    enum EIconPassiveAnimation
    {
        None, 
        Jump,
        Squeeze
    }

    enum EIconActiveAnimation
    {
        None,
        FallBounce,
        Disappearing
    }

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
        protected EIconPassiveAnimation myPassiveAnimation = EIconPassiveAnimation.None;
        protected EIconActiveAnimation myActiveAnimation = EIconActiveAnimation.None;

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

        public void UpdateIconAnimation(bool aIsExceedingRoof)
        {
            EIconPassiveAnimation ResolvedIconAnimation = EIconPassiveAnimation.None;

            if (aIsExceedingRoof)
                ResolvedIconAnimation = EIconPassiveAnimation.Squeeze;

            myPassiveAnimation = ResolvedIconAnimation;
            //etc etc
        }

        protected void SetIconAnimation()
        {
            if (myActiveAnimation != EIconActiveAnimation.None)
            {
                // do active animation stuff
            }
            else
            {
                switch (myPassiveAnimation)
                {
                    case EIconPassiveAnimation.Squeeze:
                        myIcon.SetScale(new Vector2(1.1f, 0.8f));
                        myIcon.SetPosition(myIcon.GetPosition() + new Vector2(-GetTileSize() * 0.05f, GetTileSize() * 0.2f));
                        break;

                    case EIconPassiveAnimation.None:
                        myIcon.SetScale(new Vector2(1.0f, 1.0f));
                        break;

                    default:
                        break;
                }
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset) 
        {
            mySprite.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset));
            mySprite.Draw(aSpriteBatch);

            if (myIcon == null)
                return;

            myIcon.SetPosition(GetScreenPosition(aGridOffset, aGridHeight, aRaisingOffset));
            SetIconAnimation();
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
