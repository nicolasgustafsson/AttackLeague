using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackLeague.Utility.Sprites;

namespace AttackLeague.Utility.GUI
{
    public delegate bool ButtonAction();

    class Button
    {
        public Sprite mySprite;

        public ButtonAction OnClicked;    

        void Update()
        {
            
        }
    }
}
