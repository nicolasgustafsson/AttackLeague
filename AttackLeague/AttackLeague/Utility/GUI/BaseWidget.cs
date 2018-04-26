using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.GUI
{
    public enum VerticalAlignment
    {
        Top,
        Down,
        Center
    }

    public enum HorizontalAlignment
    {
        Left,
        Right,
        Center
    }

    class BaseWidget
    {
        protected BaseWidget myParent;
        protected List<BaseWidget> myChildren;
        protected Vector2 myPivot;
        protected Vector2 myPosition;
        protected Vector2 mySize;

        public BaseWidget()
        {
            myParent = null;
            myChildren = new List<BaseWidget>();
            myPivot = new Vector2();
            myPosition = new Vector2();
            mySize = new Vector2();
        }

        //this made private for DRY purposes
        private void SetParent(BaseWidget aParent)
        {
            Debug.Assert(myChildren.Find(x => x == aParent) == null, "Cannot have your own child as parent, you wicked person!");
            myParent = aParent;
        }

        public virtual void AddChild(BaseWidget aWidget)
        {
            Debug.Assert(aWidget != this, "Cannot have your own parent as child, you sick human being!");

            myChildren.Add(aWidget);
            aWidget.SetParent(this);
        }

        public virtual void SetPivot(Vector2 aPivot)
        {
            myPivot = aPivot;
        }

        public virtual Vector2 GetPivot()
        {
            return myPivot;
        }

        public virtual void SetPosition(Vector2 aPosition)
        {
            myPosition = aPosition;

            //what we do with child?!??!?!!?!?!?!?!?!?!?!?!!+1+1+11+?!+1+1?!+!?1?1+!2+1!+!?!+!+1+!?!?1+?!+!?!+!??!?!+!+!+!?!!?1+?!+!?1++!+!?"1+!??!+1
        }

        public virtual Vector2 GetPosition()
        {
            return myPosition;
        }

        public virtual void SetSize(Vector2 aSize)
        {
            mySize = aSize;
        }

        public virtual Vector2 GetSize()
        {
            return mySize;
        }

    }
}
/*
 WHERE WILL WE HAVE GUI AND HOW WILL IT WORK
    menus
        
    interpolations for
        pos
        rot
        scale
        color
        alpha

    other stuff liek
        particles
        sounds

    cool squishy squiggly feedback

    column/row widget container - canvas
    Button
    Dropdown
    Textbox
    
    Options menu!
        Checkbox
        Sliders?

     
     */
