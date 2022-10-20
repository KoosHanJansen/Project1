using Microsoft.Xna.Framework;

namespace Project1
{
    class Velocity
    {
        public Vector2 speed;

        public Velocity()
        {
            speed = new Vector2(0, 0);
        }

        public Velocity(Vector2 initialSpeed)
        {
            speed = initialSpeed;
        }
    }
}
