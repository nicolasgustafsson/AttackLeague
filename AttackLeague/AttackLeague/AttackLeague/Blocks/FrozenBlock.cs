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

    class FrozenBlock : AbstractBlock
    {
       private Color myColor;
       public FrozenBlock(Color aColor)
       {
            myColor = aColor * 0.3f;
            myColor.A = 255;
            myGridArea = new Rectangle(0, 0, 1, 1);
        }

        public override void LoadContent(ContentManager aContent)
        {
            mySprite = new Sprite("ColorBlock", aContent);
            mySprite.SetColor(myColor);
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

        public override void Draw(SpriteBatch aSpriteBatch, Vector2 aGridOffset, int aGridHeight)
        {
            mySprite.SetPosition(aGridOffset + new Vector2(myGridArea.X * mySprite.GetSize().X, (aGridHeight - myGridArea.Y) * mySprite.GetSize().Y));
            mySprite.Draw(aSpriteBatch);
        }
    }
}
