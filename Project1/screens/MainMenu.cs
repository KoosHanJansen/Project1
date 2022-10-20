﻿using System;
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
        private OrthographicCamera camera;

        private World world;
        private World uiContainer;

        private MouseInfo mouseInfo;
        private Entity playBtn;
        private Entity newGame;

        public override void Initialize()
        {
            base.Initialize();

            camera = new OrthographicCamera(Game1.viewportAdapter);

            world = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice, camera))
                .Build();

            uiContainer = new WorldBuilder()
                .AddSystem(new MouseControl(camera))
                .AddSystem(new UIRenderer(GraphicsDevice))
                .AddSystem(new UITextRenderer())
                .AddSystem(new UIInputHandler())
                .Build();

            Game.Components.Add(world);
            Game.Components.Add(uiContainer);

            mouseInfo = new MouseInfo();

            Button playButton = new Button();
            playButton.ButtonPress += OnPlayButtonPressed;

            playBtn = uiContainer.CreateEntity();
            playBtn.Attach(new Transform2(Game.VIRTUAL_CENTER));
            playBtn.Attach(playButton);
            playBtn.Attach(mouseInfo);

            //Text test
            newGame = uiContainer.CreateEntity();
        }

        public MainMenu(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
            playBtn.Attach(new Sprite(Content.Load<Texture2D>("StartGame")));
            newGame.Attach(new Text(Content.Load<SpriteFont>("mmBigHeader"), "New Game", Game.VIRTUAL_CENTER, Color.White));
        }

        public void OnPlayButtonPressed(object e, EventArgs args)
        {
            MyGame actualGamePoggers = new MyGame(Game);
            Game.ChangeScreen(ref actualGamePoggers);
        }

        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
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
