using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.GUI
{
    class GUICaretaker
    {
        public void Update()
        {
            myGUIs.ForEach(x => x.Update());
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            myGUIs.ForEach(x => x.Draw(aSpriteBatch));
        }

        public void AddGUI(Button aGUI)
        {
            myGUIs.Add(aGUI);
        }

        public void RemoveGUI(Button aGUI)
        {
            myGUIs.Remove(aGUI);
        }

        public void SpringClean()
        {
            myGUIs.Clear(); //like the windows after spring clean
        }

        private List<Button> myGUIs = new List<Button>();
    }
}
