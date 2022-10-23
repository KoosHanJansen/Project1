using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Reflection.Emit;

namespace Project1
{
    public class InputBox
    {
        public RectangleF HitBox;
        public bool active = false;
        public int CharLimit = 20;

        public InputBox() { }
        
        public InputBox(RectangleF hitBox)
        {
            this.HitBox = hitBox;
        }

        public void OnInput(object e, TextInputEventArgs args, Text label)
        {
            if (!active)
                return;

            Keys k = args.Key;
            char c = args.Character;

            if (!k.Equals(Keys.Back) && label.font.Characters.Contains(c) && label.text.Length < CharLimit)
                label.text = label.text + c;
            else if (label.text.Length > 0 && k.Equals(Keys.Back))
                label.text = label.text.Remove(label.text.Length - 1, 1);
        }
    }
}
