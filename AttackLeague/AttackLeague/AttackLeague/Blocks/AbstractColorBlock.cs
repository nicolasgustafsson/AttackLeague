using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.AttackLeague
{
    enum EBlockColor
    {
        Cyan,
        Magenta,
        Yellow,
        Red,
        Green,
        Blue,
        Grey,
        None
    }

    abstract class AbstractColorBlock : AbstractBlock
    {
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

        public override void LoadContent(ContentManager aContent)
        {
            mySprite = new Sprite("ColorBlock", aContent);
            mySprite.SetColor(GetColorFromEnum());
        }

        public Color GetColorFromEnum()
        {
            switch (myColor)
            {
                case EBlockColor.Cyan:
                    return Color.Cyan;
                case EBlockColor.Magenta:
                    return Color.Magenta;
                case EBlockColor.Yellow:
                    return Color.Yellow;
                case EBlockColor.Red:
                    return Color.Red;
                case EBlockColor.Green:
                    return Color.Chartreuse;
                case EBlockColor.Blue:
                    return Color.DarkBlue;
                case EBlockColor.Grey:
                    return Color.HotPink;
                default:
                    return Color.White;
            }
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
