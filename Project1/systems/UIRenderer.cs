using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace Project1
{
    class UIRenderer : EntityDrawSystem
    {
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private ComponentMapper<Sprite> spriteMapper;
        private ComponentMapper<Transform2> transformMapper;

        private readonly float VIRTUAL_SCREEN_WIDTH;
        private readonly float VIRTUAL_SCREEN_HEIGHT;

        private Matrix matrix;

        public UIRenderer(GraphicsDevice graphicsDevice, float virtualWidth, float virtualHeight)
            : base(Aspect.All(typeof(Sprite), typeof(Transform2)))
        {
            this.graphicsDevice = graphicsDevice;
            this.VIRTUAL_SCREEN_WIDTH = virtualWidth;
            this.VIRTUAL_SCREEN_HEIGHT = virtualHeight;
            
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        private void SetScaleMatrix()
        {
            float scaleX = (float)graphicsDevice.Viewport.Width / VIRTUAL_SCREEN_WIDTH;
            float scaleY = (float)graphicsDevice.Viewport.Height / VIRTUAL_SCREEN_HEIGHT;
            matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            SetScaleMatrix();

            spriteMapper = mapperService.GetMapper<Sprite>();
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: matrix);
            
            foreach(var entity in ActiveEntities)
            {
                Transform2 transform = transformMapper.Get(entity);
                Sprite sprite = spriteMapper.Get(entity);

                spriteBatch.Draw(sprite, transform);
            }

            spriteBatch.End();
        }
    }
}
