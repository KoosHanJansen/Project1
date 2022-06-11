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
        private World uiContainer;

        private Entity button;

        public override void Initialize()
        {
            base.Initialize();

            world = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT))
                .Build();

            uiContainer = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT))
                .Build();

            Game.Components.Add(world);
            Game.Components.Add(uiContainer);

            button = uiContainer.CreateEntity();
            button.Attach(new Transform2(Vector2.Zero));
        }

        public MainMenu(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
            button.Attach(new Sprite(Content.Load<Texture2D>("TestPNG64x64")));
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
            uiContainer.Draw(gameTime);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            world.Dispose();
            uiContainer.Dispose();

            Game.Components.Remove(world);
            Game.Components.Remove(uiContainer);
        }
    }
}
