using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Project1
{
    class MouseControl : EntityUpdateSystem
    {
        private OrthographicCamera camera;

        private ComponentMapper<MouseInfo> mouseInfoMapper;

        public MouseControl(OrthographicCamera camera)
            : base(Aspect.All(typeof(MouseInfo)))
        {
            this.camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            mouseInfoMapper = mapperService.GetMapper<MouseInfo>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                MouseInfo mouse = mouseInfoMapper.Get(entity);

                MouseState mouseState = Mouse.GetState();
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

                Matrix uiMatrix = Game1.viewportAdapter.GetScaleMatrix();

                mouse.position = Vector2.Transform(mousePos, camera.GetInverseViewMatrix());
                mouse.localPosition = Vector2.Transform(mousePos, Matrix.Invert(uiMatrix));

                mouse.leftButton = mouseState.LeftButton == ButtonState.Pressed;
                mouse.rightButton = mouseState.RightButton == ButtonState.Pressed;
                mouse.middleButton = mouseState.MiddleButton == ButtonState.Pressed;

                mouse.scrollWheel = mouseState.ScrollWheelValue;
            }
        }
    }
}
