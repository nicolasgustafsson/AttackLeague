using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackLeague.Utility.GUI
{
    [Serializable]
    class Text : BaseGUI
    {
        public Vector2 myPosition = Vector2.Zero;
        public string myText = "";
        public Color myColor = Color.Black;

        [NonSerialized]
        SpriteFont myFont;

        public Text()
            : base()
        {
            myFont = ContentManagerInstance.Content.Load<SpriteFont>("raditascartoon");
        }

        public override void Draw(SpriteBatch aSpriteBatch) 
        {
            if (myVisibility == EGUIVisibility.Hidden)
                return;
            aSpriteBatch.DrawString(myFont, myText, myPosition, myColor);
        }

        public override void Update()
        {
            Console.WriteLine("Hej ylfmeister");
        }
    }
}
