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
    class ColorBlock : AbstractColorBlock
    {

        public ColorBlock()
        {
        }

        public ColorBlock(EBlockColor aColor)
        {
            myColor = aColor;
        }

        public override void Update()
        {
        }
    }
}
