using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Project1
{
    class InputBox
    {
        public delegate void OnInputEventHandler(object source, TextInputEventArgs args);

        public event OnInputEventHandler Input;

        public RectangleF HitBox;

        public InputBox() { }
        
        public InputBox(RectangleF hitBox)
        {
            this.HitBox = hitBox;
        }

        public virtual void OnInput()
        {
            if (Input != null)
            {
                Input(this, new TextInputEventArgs());
            }
        }
    }
}
