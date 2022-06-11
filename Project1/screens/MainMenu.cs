using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project1
{
    class MainMenu : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        private World world;
        private GuiSystem guiSystem;

        public override void Initialize()
        {
            base.Initialize();

            world = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT))
                .Build();

            Game.Components.Add(world);

            guiSystem = new GuiSystem(Game);
            guiSystem.AddElement(new Button(new Transform2(new Vector2(100, 100))));
            
            guiSystem.InitializeElements();
        }

        public MainMenu(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
            guiSystem.LoadContentOfElements();
        }

        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                MyGame actualGamePoggers = new MyGame(Game);
                Game.ChangeScreen(ref actualGamePoggers);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            world.Draw(gameTime);
            guiSystem.DrawElements();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            world.Dispose();
            Game.Components.Remove(world);
        }
    }
}
