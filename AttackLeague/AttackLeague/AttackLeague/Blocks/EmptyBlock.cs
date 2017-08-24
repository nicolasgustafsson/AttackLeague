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

    class EmptyBlock : AbstractBlock
    {

        public EmptyBlock()
        {
            myGridArea = new Rectangle(0, 0, 1, 1);
        }

        public void SetPosition(int aX, int aY)
        {
            myGridArea.X = aX;
            myGridArea.Y = aY;
        }

        public override void LoadContent(ContentManager aContent)
        {
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
        }
    }
}
