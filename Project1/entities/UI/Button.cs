using MonoGame.Extended;
using MonoGame.Extended.Entities;
using System;

namespace Project1
{
    class Button
    {
        public delegate void ButtonPressEventHandler(object source, EventArgs args);
        public delegate void MouseOverEventHandler(object source, EventArgs args);
        public delegate void MouseExitEventHandler(object source, EventArgs args);

        public event ButtonPressEventHandler ButtonPress;
        public event ButtonPressEventHandler MouseOver;
        public event ButtonPressEventHandler MouseExit;

        private RectangleF hitBox;
        public RectangleF HitBox { get { return this.hitBox; } set { this.hitBox = value; } }

        public bool Active = true;

        public virtual void OnButtonPress()
        {
            if (ButtonPress != null)
            {
                ButtonPress(this, EventArgs.Empty);
            }
        }

        public virtual void OnMouseOver()
        {
            if (MouseOver != null)
            {
                MouseOver(this, EventArgs.Empty);
            }
        }
        
        public virtual void OnMouseExit()
        {
            if (MouseExit != null)
            {
                MouseExit(this, EventArgs.Empty);
            }
        }
    }
}
