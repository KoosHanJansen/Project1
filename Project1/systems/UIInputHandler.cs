using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    class UIInputHandler : EntityUpdateSystem
    {
        private GraphicsDevice graphicsDevice;

        private ComponentMapper<Sprite> spriteMapper;
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<Button> buttonMapper;

        private readonly float VIRTUAL_SCREEN_WIDTH;
        private readonly float VIRTUAL_SCREEN_HEIGHT;

        private Matrix scaleMatrix;

        public UIInputHandler(GraphicsDevice graphicsDevice, float virtualWidth, float virtualHeight)
            : base (Aspect.All(typeof(Button), typeof(Sprite), typeof(Transform2)))
        {
            this.graphicsDevice = graphicsDevice;

            this.VIRTUAL_SCREEN_WIDTH = virtualWidth;
            this.VIRTUAL_SCREEN_HEIGHT = virtualHeight;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            spriteMapper = mapperService.GetMapper<Sprite>();
            transformMapper = mapperService.GetMapper<Transform2>();
            buttonMapper = mapperService.GetMapper<Button>();

            SetScaleMatrix();
        }

        private void SetScaleMatrix()
        {
            float scaleX = (float)graphicsDevice.Viewport.Width / VIRTUAL_SCREEN_WIDTH;
            float scaleY = (float)graphicsDevice.Viewport.Height / VIRTUAL_SCREEN_HEIGHT;
            scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 scaledMousePos = Vector2.Transform(mousePosition, Matrix.Invert(scaleMatrix));

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                foreach (var entity in ActiveEntities)
                {
                    Transform2 transform = transformMapper.Get(entity);
                    Sprite sprite = spriteMapper.Get(entity);
                    Button button = buttonMapper.Get(entity);

                    RectangleF hitBox = sprite.GetBoundingRectangle(transform);

                    if (hitBox.Contains(scaledMousePos))
                    {
                        button.OnButtonPress();
                    }
                }
            }

        }
    }
}
