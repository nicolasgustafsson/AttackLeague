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

    public class FrozenBlock : AbstractColorBlock
    {
        public FrozenBlock()
        {
        }

        public FrozenBlock(EBlockColor aColor)
        {
            myColor = aColor;
        }

        public override void RandomizeColor()
        {
            myColor = (EBlockColor)Randomizer.GlobalRandomizer.Next(0, 5);
            if (mySprite != null)
            {
                UpdateColor();
            }
        }

        public override void LoadContent()
        {
            mySprite = new Sprite("tiley");
            UpdateColor();
        }

        private void UpdateColor()
        {
            Color color = GetColorFromEnum() * 0.3f;
            color.A = 255;
            mySprite.SetColor(color);
            myIcon = GetIconFromEnum();
        }

        public override void Update(float aGameSpeed)
        {
        }
    }
}
