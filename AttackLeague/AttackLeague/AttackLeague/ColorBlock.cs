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

    enum BlockColor
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
        private BlockColor myColor = (BlockColor)Randomizer.GlobalRandomizer.Next(0, 5);

        public ColorBlock()
        {
            myGridArea = new Rectangle(0, 0, 1, 1);
        }

        public override void LoadContent(ContentManager aContent)
        {
            mySprite = new Sprite("ColorBlock", aContent);
            mySprite.SetColor(GetColorFromEnum());

        }

        private Color GetColorFromEnum()
        {
            switch (myColor)
            {
                case BlockColor.Cyan:
                    return Color.Cyan;
                case BlockColor.Magenta:
                    return Color.Magenta;
                case BlockColor.Yellow:
                    return Color.Yellow;
                case BlockColor.Red:
                    return Color.Red;
                case BlockColor.Green:
                    return Color.Green;
                case BlockColor.Blue:
                    return Color.DarkBlue;
                case BlockColor.Grey:
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

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset)
        {
            mySprite.SetPosition(aGridOffset + new Vector2(myGridArea.X, myGridArea.Y) * mySprite.GetSize());
            mySprite.Draw(aSpriteBatch);
        }

        public BlockColor GetColor()
        {
            return myColor;
        }
    }
}
