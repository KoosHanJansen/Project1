using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace Project1
{
    class ComponentRenderer : EntityDrawSystem
    {
        private SpriteBatch spriteBatch;

        private OrthographicCamera camera;

        private ComponentMapper<Sprite> spriteMapper;
        private ComponentMapper<Transform2> transformMapper;

        public ComponentRenderer(GraphicsDevice graphicsDevice, OrthographicCamera camera)
            : base(Aspect.All(typeof(Sprite), typeof(Transform2)))
        {
            spriteBatch = new SpriteBatch(graphicsDevice);

            if (camera != null)
                this.camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            spriteMapper = mapperService.GetMapper<Sprite>();
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix cam = camera.GetViewMatrix(Vector2.One);

            spriteBatch.Begin(transformMatrix: cam);
            
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
