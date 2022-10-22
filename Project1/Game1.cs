using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Project1.libs;

namespace Project1
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        public static GameViewport viewportAdapter;
        public static Time time;
        public static MouseInfo mouseInfo;
        public static OrthographicCamera camera;
        public SpriteBatch SpriteBatch;

        private Settings gameSettings;
        private readonly ScreenManager screenManager;

        public readonly float VIRTUAL_WIDTH;
        public readonly float VIRTUAL_HEIGHT;

        public readonly Vector2 VIRTUAL_CENTER;

        public Game1()
        {
            VIRTUAL_WIDTH = 1920;
            VIRTUAL_HEIGHT = 1080;

            VIRTUAL_CENTER = new Vector2(VIRTUAL_WIDTH * 0.5f, VIRTUAL_HEIGHT * 0.5f);

            graphics = new GraphicsDeviceManager(this);
            viewportAdapter = new GameViewport((int)VIRTUAL_WIDTH, (int)VIRTUAL_HEIGHT, GraphicsDevice);
            time = new Time();
            mouseInfo = new MouseInfo();
            camera = new OrthographicCamera(viewportAdapter);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            screenManager = new ScreenManager();
            Components.Add(screenManager);
        }

        public void ChangeScreen<T>(ref T screen) where T : GameScreen
        {
            screenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black));
        }

        protected override void Initialize()
        {
            if (GraphicsDevice == null)
                graphics.ApplyChanges();
           
            gameSettings = new Settings(graphics, Window);

            SplashScreen splashScreen = new SplashScreen(this);
            ChangeScreen(ref splashScreen);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
                graphics.ToggleFullScreen();

            base.Update(gameTime);
            time.Update();
            UpdateMouseInfo();
        }

        protected void UpdateMouseInfo()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

            Matrix uiMatrix = viewportAdapter.GetScaleMatrix();

            mouseInfo.position = Vector2.Transform(mousePos, camera.GetInverseViewMatrix());
            mouseInfo.localPosition = Vector2.Transform(mousePos, Matrix.Invert(uiMatrix));

            mouseInfo.leftButton = mouseState.LeftButton == ButtonState.Pressed;
            mouseInfo.rightButton = mouseState.RightButton == ButtonState.Pressed;
            mouseInfo.middleButton = mouseState.MiddleButton == ButtonState.Pressed;

            mouseInfo.scrollWheel = mouseState.ScrollWheelValue;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
