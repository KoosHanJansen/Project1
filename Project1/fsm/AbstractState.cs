using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Project1
{
    public class AbstractState<T> where T : AbstractObject
    {
        protected T target;

        public AbstractState(T obj)
        {
            target = obj;
        }

        public virtual void Enter()
        {
            Console.WriteLine("Entered state: " + this);
        }

        public virtual void Update(GameTime gameTime)
        {
            Console.WriteLine("Updating state: " + this);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Console.WriteLine("Drawing state: " + this);
        }

        public virtual void Exit()
        {
            Console.WriteLine("Exited state: " + this);
        }
    }
}
