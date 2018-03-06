using AttackLeague.Utility.GUI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.StateStack
{
    class State
    {
        // TODO add gui container
        public StateStack myStateStack { protected get; set; }

        protected GUICaretaker myGUICaretaker = new GUICaretaker();

        /*
         Main menu
         Game - renderas (jag har ingen transprens)
            Pause menu - transparency, vill rendera game under
         Controllers - independent på andras rendering (har du transparans?)

            update through
            render through
             */

        public State()
        {
            UpdateThrough = false;
            RenderThrough = false;
        }

        public void StateUpdate()
        {
            Update();

            GUIUpdate();
        }

        protected virtual void Update()
        {
            //haha
        }

        protected virtual void GUIUpdate()
        {
            myGUICaretaker.Update();
        }

        public void StateDraw(SpriteBatch aSpriteBatch)
        {
            Draw(aSpriteBatch);
            GUIDraw(aSpriteBatch);
        }

        protected virtual void Draw(SpriteBatch aSpriteBatch)
        {
        }

        protected virtual void GUIDraw(SpriteBatch aSpriteBatch)
        {
            myGUICaretaker.Draw(aSpriteBatch);
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void OnCreated()
        {
            
        }

        public virtual void OnDestroyed()
        {
            
        }

        public bool UpdateThrough { get; set; }
        public bool RenderThrough { get; set; }
    }
}
