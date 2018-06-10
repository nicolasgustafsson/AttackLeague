using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.GUI
{
    enum EGUIVisibility
    {
        Visible,
        Hidden
    }

    [Serializable]
    abstract class BaseGUI
    {
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            myVisibility = myStartingVisibility;
        }


        public abstract void Update();
        public abstract void Draw(SpriteBatch aSpriteBatch);

        public void SetVisibility(EGUIVisibility aNewVisibility)
        {
            myVisibility = aNewVisibility;
        }

        public EGUIVisibility myStartingVisibility = EGUIVisibility.Visible;
        protected EGUIVisibility myVisibility = EGUIVisibility.Visible;
    }
}
