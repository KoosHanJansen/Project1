using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using Project1.rendering;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Project1
{
    class MyGame : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        private OrthographicCamera camera;

        private World world;
        private World uiContainer;

        private Map map;
        private Color[,] mapData;

        private MouseInfo mouseInfo;
        private Entity player;

        private QuadTree ChunkRenderer;

        public override void Initialize()
        {
            base.Initialize();

            camera = new OrthographicCamera(Game1.viewportAdapter);
            
            world = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice, camera))
                .AddSystem(new MouseControl(camera))
                .AddSystem(new Movement())
                .AddSystem(new PlayerControl())
                .AddSystem(new PlayerInputHandler())
                .Build();

            uiContainer = new WorldBuilder()
                .AddSystem(new MouseControl(camera))
                .AddSystem(new UIRenderer(GraphicsDevice))
                .AddSystem(new UIInputHandler())
                .Build();

            Game.Components.Add(world);
            Game.Components.Add(uiContainer);

            mouseInfo = new MouseInfo();

            player = world.CreateEntity();
            player.Attach(new Transform2(Game.VIRTUAL_CENTER));
            player.Attach(new PlayerInput());
            player.Attach(new Velocity());
            player.Attach(new Player());
            player.Attach(mouseInfo);

            map = new Map();

            Map.MapSettings mSettings = new Map.MapSettings();

            Random r = new Random();

            mSettings.seed = r.Next();
            mSettings.width = 1024;
            mSettings.height = 1024;
            mSettings.borderSize = 10;
            mSettings.density = 0.55f;
            mSettings.scale = 20.0f;
            mSettings.frequency = 1.0f;

            mapData = map.GenerateMap(mSettings);

            ChunkRenderer = new QuadTree(world, Vector2.Zero, player.Get<Transform2>(), 1024, mapData, 6);
            Debug.WriteLine(camera.Origin.ToString());
        }

        public MyGame(Game1 game) : base(game) { }

        void OnTestButtonPress(object source, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu(Game);
            Game.ChangeScreen(ref mainMenu);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            player.Attach(new Sprite(Content.Load<Texture2D>("TestPNG64x64")));
            
        }

        public override void Update(GameTime gameTime)
        {
            camera.LookAt(player.Get<Transform2>().Position);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camera.ZoomIn(0.1f);
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camera.ZoomOut(0.1f);
            }

            world.Update(gameTime);
            ChunkRenderer.UpdateTree();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
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
