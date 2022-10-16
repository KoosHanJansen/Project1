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
        private ComponentMapper<Sprite> spriteMapper;
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<Button> buttonMapper;

        public UIInputHandler()
            : base (Aspect.All(typeof(Button), typeof(Sprite), typeof(Transform2)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            spriteMapper = mapperService.GetMapper<Sprite>();
            transformMapper = mapperService.GetMapper<Transform2>();
            buttonMapper = mapperService.GetMapper<Button>();
        }

        public override void Update(GameTime gameTime)
        {
            Matrix scaleMatrix = Game1.viewportAdapter.GetScaleMatrix();

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
