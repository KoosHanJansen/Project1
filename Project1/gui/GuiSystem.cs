using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project1
{
    class GuiSystem
    {
        private Game1 Game;

        private World guiContainer;

        private delegate void InitializeDelegates();
        private InitializeDelegates initializeHandler;

        private delegate void LoadContentDelegates(ContentManager Content);
        private LoadContentDelegates loadContentHandler;

        private delegate void DrawDelegates(SpriteBatch spriteBatch);
        private DrawDelegates drawHandler;

        public GuiSystem(Game1 game) 
        {
            Game = game;
            guiContainer = new WorldBuilder().AddSystem(new ComponentRenderer(Game.GraphicsDevice, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT)).Build();
        }

        public void AddElement(object e)
        {
            if (e == null) return;

            if (e is IGuiElement)
            {
                IGuiElement guiElement = e as IGuiElement;
                initializeHandler += guiElement.Initialize;
                loadContentHandler += guiElement.LoadContent;
                drawHandler += guiElement.Draw;
            }
        }

        public void RemoveElement(object e)
        {
            if (e == null) return;

            if (e is IGuiElement)
            {
                IGuiElement guiElement = e as IGuiElement;
                initializeHandler -= guiElement.Initialize;
                loadContentHandler -= guiElement.LoadContent;
                drawHandler -= guiElement.Draw;
            }
        }

        public void InitializeElements()
        {
            if (initializeHandler != null)
                initializeHandler();
        }

        public void LoadContentOfElements()
        {
            if (loadContentHandler != null)
                loadContentHandler(Game.Content);
        }

        public void DrawElements()
        {
            if (drawHandler != null)
                drawHandler(Game.SpriteBatch);
        }
    }
}
