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
        private ComponentMapper<MouseInfo> mouseInfoMapper;

        public UIInputHandler()
            : base (Aspect.All(typeof(Button), typeof(Sprite), typeof(Transform2), typeof(MouseInfo)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            spriteMapper = mapperService.GetMapper<Sprite>();
            transformMapper = mapperService.GetMapper<Transform2>();
            buttonMapper = mapperService.GetMapper<Button>();
            mouseInfoMapper = mapperService.GetMapper<MouseInfo>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                Transform2 transform = transformMapper.Get(entity);
                Sprite sprite = spriteMapper.Get(entity);
                Button button = buttonMapper.Get(entity);
                MouseInfo mouse = mouseInfoMapper.Get(entity);

                if (mouse.leftButton)
                {
                    RectangleF hitBox = sprite.GetBoundingRectangle(transform);

                    if (hitBox.Contains(mouse.localPosition))
                    {
                        button.OnButtonPress();
                    }

                    Debug.WriteLine("UI: " + mouse.localPosition.ToString());
                }
            }
        }
    }
}
