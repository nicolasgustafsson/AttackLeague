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

    class FallingBlock : AbstractBlock
    {
        private EBlockColor myColor;
        private const float BaseSpeed = 0.25f;
        protected float myYOffset = 0.0f;

        public FallingBlock(EBlockColor aColor)
        {
            myGridArea = new Rectangle(0, 0, 1, 2);
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
            myYOffset -= BaseSpeed;
        }

        public bool WillPassTile()
        {
            return myYOffset - BaseSpeed <= 0.0f;
        }

        public void PassTile()
        {
            myGridArea.Y--;
            myYOffset += 1.0f;
        }

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight, float aRaisingOffset)
        {
            mySprite.SetPosition(aGridOffset + 
                new Vector2(myGridArea.X * mySprite.GetSize().X, (
                aGridHeight - myGridArea.Y) 
                * mySprite.GetSize().Y 
                - myYOffset * mySprite.GetSize().Y
                + aRaisingOffset));
            mySprite.Draw(aSpriteBatch);
        }

        public EBlockColor GetColor()
        {
            return myColor;
        }
    }
}
