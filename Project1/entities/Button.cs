using System;
using System.Collections.Generic;
using System.Text;

namespace Project1
{
    class Button
    {
        public delegate void ButtonPressEventHandler(object source, EventArgs args);

        public event ButtonPressEventHandler ButtonPress;

        public virtual void OnButtonPress()
        {
            if (ButtonPress != null)
            {
                ButtonPress(this, EventArgs.Empty);
            }
        }
    }
}
