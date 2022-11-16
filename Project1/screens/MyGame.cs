using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using Project1.rendering;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Project1
{
    class MyGame : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private World world;
        private World uiContainer;

        private Map map;
        private Color[,] mapData;

        private Entity player;

        private QuadTree chunkTree;

        private string currentSave;

        public MyGame(Game1 game) : base(game) { }

        public MyGame(Game1 game, string save)
            : base(game)
        {
            this.currentSave = save;
        }

        public override void Initialize()
        {
            base.Initialize();

            PlayerInputHandler piHandler = new PlayerInputHandler();
            Movement movement = new Movement();

            world = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice))
                .AddSystem(movement)
                .AddSystem(new PlayerControl())
                .AddSystem(new ChunkRenderer())
                .AddSystem(piHandler)
                .Build();

            uiContainer = new WorldBuilder()
                .AddSystem(new UIRenderer(GraphicsDevice))
                .AddSystem(new UIInputHandler())
                .Build();

            Game.Components.Add(world);
            Game.Components.Add(uiContainer);

            map = new Map();
            SetMapSettings();

            if (FileLocations.Exists(FileLocations.SAVES_DIRECTORY + "\\" + currentSave))
                LoadLevel();
            else
                LoadNewLevel();         
            
            chunkTree = new QuadTree(world, Vector2.Zero, player.Get<Transform2>(), 1024, mapData, 6);
            piHandler.SetMap(chunkTree);
            movement.SetMap(chunkTree);
        }

        private void SetMapSettings()
        {
            Map.MapSettings mSettings = new Map.MapSettings();
            Random r = new Random();

            mSettings.seed = r.Next();
            mSettings.width = 1024;
            mSettings.height = 1024;
            mSettings.borderSize = 10;
            mSettings.density = 0.55f;
            mSettings.scale = 20.0f;
            mSettings.frequency = 1.0f;

            map.Settings = mSettings;
        }

        private void LoadLevel()
        {
            mapData = map.LoadMap(currentSave);
            SpawnPlayer();
        }

        private void LoadNewLevel()
        {
            mapData = map.GenerateMap();
            SpawnPlayer();
            map.SaveMap(currentSave, mapData);
        }

        private void SpawnPlayer()
        {
            player = world.CreateEntity();
            player.Attach(new Transform2(map.PlayerPosition));
            player.Attach(new PlayerInput());
            player.Attach(new Velocity());
            player.Attach(new Player());
        }

        void OnTestButtonPress(object source, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu(Game);
            Game.ChangeScreen(ref mainMenu);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            player.Attach(new Sprite(Content.Load<Texture2D>("smol_einhorn")));
        }

        public override void Update(GameTime gameTime)
        {
            Game1.camera.LookAt(player.Get<Transform2>().Position);

            Game1.camera.MaximumZoom = 8;
            Game1.camera.MinimumZoom = 0.1f;

            if (Game1.mouseInfo.Scrolled())
                Game1.camera.ZoomIn(Game1.mouseInfo.ScrollWheel() / 1200f);

            if (Keyboard.GetState().IsKeyDown(Keys.F6))
            {
                Debug.WriteLine("Saving map");
                map.SaveMap(currentSave, mapData);
            }

            chunkTree.UpdateTree();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
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
