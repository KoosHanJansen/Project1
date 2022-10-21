using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Project1
{
    class MainMenu : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        private OrthographicCamera camera;

        private World world;
        private World uiContainer;

        private MouseInfo mouseInfo;
        private Entity newGame;
        private Entity settings;

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

            newGame = uiContainer.CreateEntity();
            newGame.Attach(mouseInfo);

            settings = uiContainer.CreateEntity();
            settings.Attach(mouseInfo);
        }

        public MainMenu(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
            
            //Start Game 
            Text startGameText = new Text(Content.Load<SpriteFont>("mmBigHeader"), "Start Game", new Vector2(50, 425), Color.White);
            newGame.Attach(startGameText);
            Button sgButton = new Button();
            sgButton.ButtonPress += OnStartGameButtonPressed;
            sgButton.MouseOver += delegate (object e, EventArgs args) { OnMouseOverButton(e, args, newGame); };
            sgButton.MouseExit += delegate (object e, EventArgs args) { OnMouseExitButton(e, args, newGame); };
            sgButton.HitBox = startGameText.GetHitBox();
            newGame.Attach(sgButton);

            //Settings
            Text settingsText = new Text(Content.Load<SpriteFont>("mmSmallHeader"), "Settings", new Vector2(50, 550), Color.White);
            settings.Attach(settingsText);
            Button settingsButton = new Button();
            settingsButton.MouseOver += delegate (object e, EventArgs args) { OnMouseOverButton(e, args, settings); };
            settingsButton.MouseExit += delegate (object e, EventArgs args) { OnMouseExitButton(e, args, settings); };
            settingsButton.HitBox = settingsText.GetHitBox();
            settings.Attach(settingsButton);
        }

        public void OnMouseExitButton(object e, EventArgs args, Entity entity)
        {
            entity.Get<Text>().color = Color.White;
        }

        public void OnMouseOverButton(object e, EventArgs args, Entity entity)
        {
            entity.Get<Text>().color = Color.Red;
        }

        public void OnStartGameButtonPressed(object e, EventArgs args)
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
