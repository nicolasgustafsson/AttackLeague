using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
    }

    class ColorBlock : AbstractBlock
    {
        private EBlockColor myColor = (EBlockColor)Randomizer.GlobalRandomizer.Next(0, 5);

        public ColorBlock()
        {
            myGridArea = new Rectangle(0, 0, 1, 1);
        }

        public ColorBlock(EBlockColor aColor)
        {
            myGridArea = new Rectangle(0, 0, 1, 1);
            myColor = aColor;
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

        public override void Update()
        {
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            mySprite.SetPosition(aGridOffset + new Vector2(myGridArea.X * mySprite.GetSize().X, aRaisingOffset + (aGridHeight - myGridArea.Y) * mySprite.GetSize().Y));
            mySprite.Draw(aSpriteBatch);
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
