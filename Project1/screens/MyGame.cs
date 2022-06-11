using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using Project1.rendering;

namespace Project1
{
    class MyGame : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        private World world;
        private GuiSystem guiSystem;

        private Button testButton;

        private Entity player;

        private QuadTree ChunkRenderer;

        public override void Initialize()
        {
            base.Initialize();

            world = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT))
                .AddSystem(new Movement())
                .AddSystem(new PlayerControl())
                .AddSystem(new PlayerInputHandler())                
                .Build();

            Game.Components.Add(world);

            player = world.CreateEntity();
            player.Attach(new Transform2(Game.VIRTUAL_CENTER));
            player.Attach(new PlayerInput());
            player.Attach(new Velocity());
            player.Attach(new Player());

            guiSystem = new GuiSystem(Game);
            testButton = new Button(new Transform2(0, 0));

            guiSystem.AddElement(testButton);
            guiSystem.InitializeElements();

            ChunkRenderer = new QuadTree(world, Vector2.Zero, player.Get<Transform2>(), 2048);
        }

        public MyGame(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
            guiSystem.LoadContentOfElements();

            player.Attach(new Sprite(Content.Load<Texture2D>("TestPNG64x64")));
        }

        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            ChunkRenderer.UpdateTree();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
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
