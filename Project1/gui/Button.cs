using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Project1
{
    class Button : IGuiElement
    {
        Sprite sprite;

        public Transform2 transform { get; set; }

        public Button(Transform2 transform)
        {
            this.transform = transform;
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager Content)
        {
            sprite = new Sprite(Content.Load<Texture2D>("TestPNG64x64"));
            transform.Position += new Microsoft.Xna.Framework.Vector2(sprite.Origin.X, sprite.Origin.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            //sprite.Draw(spriteBatch, transform.Position, transform.Rotation, transform.Scale);
            spriteBatch.Draw(sprite, transform);
            spriteBatch.End();
        }
    }
}
