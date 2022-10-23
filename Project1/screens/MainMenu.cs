using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Project1
{
    class MainMenu : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private World world;
        private World uiContainer;

        private Entity startGame;
        private Entity settings;
        private Entity inputBox;

        public override void Initialize()
        {
            base.Initialize();

            world = new WorldBuilder()
                .AddSystem(new ComponentRenderer(GraphicsDevice))
                .Build();

            uiContainer = new WorldBuilder()
                .AddSystem(new UIRenderer(GraphicsDevice))
                .AddSystem(new UITextRenderer())
                .AddSystem(new UIInputHandler())
                .Build();

            Game.Components.Add(world);
            Game.Components.Add(uiContainer);

            startGame = uiContainer.CreateEntity();

            settings = uiContainer.CreateEntity();

            inputBox = uiContainer.CreateEntity();

            inputBox.Attach(new InputBox(new RectangleF(Game.VIRTUAL_CENTER.X, Game.VIRTUAL_CENTER.Y, 500, 100)));
        }

        public MainMenu(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
            
            //Start Game 
            Text startGameText = new Text(Content.Load<SpriteFont>("mmBigHeader"), "Start Game", new Vector2(50, 425), Color.White);
            startGame.Attach(startGameText);
            Button sgButton = new Button();
            sgButton.ButtonPress += OnStartGameButtonPressed;
            sgButton.MouseOver += delegate (object e, EventArgs args) { OnMouseOverButton(e, args, startGameText); };
            sgButton.MouseExit += delegate (object e, EventArgs args) { OnMouseExitButton(e, args, startGameText); };
            sgButton.HitBox = startGameText.GetHitBox();
            startGame.Attach(sgButton);

            //Settings
            Text settingsText = new Text(Content.Load<SpriteFont>("mmSmallHeader"), "Settings", new Vector2(50, 550), Color.White);
            settings.Attach(settingsText);
            Button settingsButton = new Button();
            settingsButton.MouseOver += delegate (object e, EventArgs args) { OnMouseOverButton(e, args, settingsText); };
            settingsButton.MouseExit += delegate (object e, EventArgs args) { OnMouseExitButton(e, args, settingsText); };
            settingsButton.HitBox = settingsText.GetHitBox();
            settings.Attach(settingsButton);

            //Inputbox
            Text inputBoxText = new Text(Content.Load<SpriteFont>("mmSmallHeader"), "", Game.VIRTUAL_CENTER, Color.White);
            inputBox.Attach(inputBoxText); 
            Game.Window.TextInput += delegate (object e, TextInputEventArgs args) { inputBox.Get<InputBox>().OnInput(e, args, inputBoxText); };
        }

        public void OnMouseExitButton(object e, EventArgs args, Text text)
        {
            text.color = Color.White;
        }

        public void OnMouseOverButton(object e, EventArgs args, Text text)
        {
            text.color = Color.Red;
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
