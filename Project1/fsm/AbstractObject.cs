using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1
{
    public abstract class AbstractObject
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        virtual public void Update(GameTime gameTime)
        {

        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
