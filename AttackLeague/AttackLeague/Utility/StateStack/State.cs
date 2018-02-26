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

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch aSpriteBatch)
        {

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
