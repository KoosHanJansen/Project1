using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Project1
{
    interface IGuiElement
    {
        Transform2 transform { get; set; }

        void Initialize();

        void LoadContent(ContentManager Content);

        void Draw(SpriteBatch spriteBatch);
    }
}
