﻿using Microsoft.Xna.Framework.Graphics;
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

        public void AddGUI(BaseGUI aGUI, string aName)
        {
            if (myGUIs.ContainsKey(aName))
            {
                Console.WriteLine("[ERROR] We have already added the GUI " + aName);
                return;
            }
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

        public BaseGUI GetGUI(String aGUIName)
        {
            return myGUIs[aGUIName];
        }

        public T GetGUI<T>(String aGUIName) where T : BaseGUI
        {
            return (T)myGUIs[aGUIName];
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            foreach(var GUIPath in myGUIPaths)
            {
                myGUIs.Add(GUIPath, JsonUtility.LoadJsonTyped(GUIPath) as BaseGUI);
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

        private Dictionary<String, BaseGUI> myGUIs = new Dictionary<String, BaseGUI>();
    }
}
