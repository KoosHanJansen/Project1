﻿using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using Project1.rendering;
using System.Diagnostics;

namespace Project1
{
    class MyGame : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        private World world;
        private World uiContainer;

        private Entity player;
        private Entity button;

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

            uiContainer = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT))
                .AddSystem(new UIInputHandler(GraphicsDevice, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT))
                .Build();

            Game.Components.Add(world);
            Game.Components.Add(uiContainer);

            player = world.CreateEntity();
            player.Attach(new Transform2(Game.VIRTUAL_CENTER));
            player.Attach(new PlayerInput());
            player.Attach(new Velocity());
            player.Attach(new Player());

            button = uiContainer.CreateEntity();
            button.Attach(new Transform2(new Vector2(100,100)));
            Button testButton = new Button();
            testButton.ButtonPress += OnTestButtonPress;
            button.Attach(testButton);

            ChunkRenderer = new QuadTree(world, Vector2.Zero, player.Get<Transform2>(), 2048);
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
            button.Attach(new Sprite(Content.Load<Texture2D>("TestPNG64x64")));
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
