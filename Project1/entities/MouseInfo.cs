using Microsoft.Xna.Framework;

namespace Project1
{
    public class MouseInfo
    {
        public Vector2 position;
        public Vector2 localPosition;

        public bool leftButton;
        public bool rightButton;
        public bool middleButton;

        public int scrollWheel;
        private int lastScroll = 0;

        public bool Scrolled()
        {
            return lastScroll != scrollWheel;
        }

        public int ScrollWheel()
        {
            int value = scrollWheel - lastScroll;
            lastScroll = scrollWheel;
            
            return value;
        }
    }
}
