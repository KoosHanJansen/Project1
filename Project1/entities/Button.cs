using MonoGame.Extended;
using System;

namespace Project1
{
    class Button
    {
        public delegate void ButtonPressEventHandler(object source, EventArgs args);

        public event ButtonPressEventHandler ButtonPress;

        private RectangleF hitBox;
        public RectangleF HitBox { get { return this.hitBox; } set { this.hitBox = value; } }

        public virtual void OnButtonPress()
        {
            if (ButtonPress != null)
            {
                ButtonPress(this, EventArgs.Empty);
            }
        }
    }
}
