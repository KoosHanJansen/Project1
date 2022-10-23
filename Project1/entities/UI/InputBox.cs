using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Project1
{
    public class InputBox
    {
        public delegate void OnInputEventHandler(object source, TextInputEventArgs args);

        public event OnInputEventHandler Input;

        public RectangleF HitBox;

        public InputBox() { }
        
        public InputBox(RectangleF hitBox)
        {
            this.HitBox = hitBox;
        }

        public virtual void OnInput(TextInputEventArgs e)
        {

            if (Input != null)
            {
                Input(this, e);
            }
        }
    }
}
