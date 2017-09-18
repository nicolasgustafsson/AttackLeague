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
        private bool myCanChain = false;


        public ColorBlock()
        {
        }

        public ColorBlock(EBlockColor aColor)
        {
            myColor = aColor;
        }

        public ColorBlock(FallingBlock aBlock)
        {
            myColor = aBlock.GetColor();
            CanChain = aBlock.CanChain;
        }

        public override void Update()
        {
        }

        public bool CanChain
        {
            get { return myCanChain; }
            set { myCanChain = value; }
        }
    }
}
