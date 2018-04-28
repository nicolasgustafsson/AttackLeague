using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.GUI
{
    [Serializable]
    class GUICaretaker
    {
        public void Update()
        {
            foreach (var Thing in myGUIs)
            {
                Thing.Value.Update();
            }
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach(var Thing in myGUIs)
            {
                Thing.Value.Draw(aSpriteBatch);
            }
        }

        public void AddGUI(Button aGUI, string aName)
        {
            myGUIs.Add(aName, aGUI);
            myGUIPaths.Add(aName);
        }

        public void RemoveGUI(string aName)
        {
            myGUIs.Remove(aName);
            myGUIPaths.Remove(aName);
        }

        public void SpringClean()
        {
            myGUIs.Clear(); //like the windows after spring clean
        }

        public Button GetButton(String aButtonName)
        {
            return myGUIs[aButtonName];
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach(var GUIPath in myGUIPaths)
            {
                myGUIs.Add(GUIPath, JsonUtility.LoadJson<Button>(GUIPath));
            }
        }

        [OnSerialized]
        internal void OnSerialized(StreamingContext context)
        {
            foreach (var Thing in myGUIs)
            {
                JsonUtility.SaveJson(Thing.Key, Thing.Value);
            }
        }

        public List<String> myGUIPaths = new List<String>();

        private Dictionary<String, Button> myGUIs = new Dictionary<String, Button>();
    }
}
