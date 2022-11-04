using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Project1
{
    class UITextRenderer : EntityDrawSystem
    {
        private SpriteBatch spriteBatch;

        private ComponentMapper<Text> textMapper;

        public UITextRenderer()
            : base(Aspect.All(typeof(Text)))
        {
            spriteBatch = new SpriteBatch(Game1.graphics.GraphicsDevice);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            textMapper = mapperService.GetMapper<Text>();
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix matrix = Game1.viewportAdapter.GetScaleMatrix();
            spriteBatch.Begin(transformMatrix: matrix);

            foreach (var entity in ActiveEntities)
            {
                Text text = textMapper.Get(entity);

                if (text.font == null || text.hide)
                    continue;

                spriteBatch.DrawString(text.font, text.text, text.position, text.color);
            }

            spriteBatch.End();
        }
    }
}
