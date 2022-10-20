﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    class Text
    {
        public SpriteFont font = null;
        public string text = "";
        public Vector2 position = Vector2.Zero;
        public Color color = Color.White;
        
        /// <summary>
        /// Text for UI
        /// </summary>
        /// <param name="font">SpriteFont loaded from content</param>
        /// <param name="text">Text as a string</param>
        /// <param name="position">Position of displayed text as a Vector2</param>
        /// <param name="color">Color of displayed text</param>
        public Text(SpriteFont font, string text, Vector2 position, Color color)
        {
            this.font = font;
            this.text = text;
            this.position = position;
            this.color = color;
        }
    }
}