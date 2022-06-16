using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Sprites;
using System;

namespace Project1
{
    class SplashScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        private OrthographicCamera camera;

        private World world;

        private Entity splash;

        private float splashTime = 3;

        public SplashScreen(Game1 game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            camera = new OrthographicCamera(GraphicsDevice);

            world = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT, camera))
                .Build();

            Game.Components.Add(world);            
        }

        public override void LoadContent()
        {
            base.LoadContent();

            splash = world.CreateEntity();
            splash.Attach(new Sprite(Content.Load<Texture2D>("Splash")));
            splash.Attach(new Transform2(Game.VIRTUAL_CENTER));
        }

        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);

            splashTime = MathF.Max(0, splashTime - gameTime.GetElapsedSeconds());

            if (splashTime == 0)
            {
                MainMenu mainMenu = new MainMenu(Game);
                Game.ChangeScreen(ref mainMenu);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            world.Draw(gameTime);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            world.DestroyEntity(splash);
            world.Dispose();
            Game.Components.Remove(world);
        }
    }
}
