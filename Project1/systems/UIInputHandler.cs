using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Diagnostics;

namespace Project1
{
    class UIInputHandler : EntityUpdateSystem
    {
        private ComponentMapper<Button> buttonMapper;
        private ComponentMapper<MouseInfo> mouseInfoMapper;

        public UIInputHandler()
            : base (Aspect.All(typeof(Button), typeof(MouseInfo)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            buttonMapper = mapperService.GetMapper<Button>();
            mouseInfoMapper = mapperService.GetMapper<MouseInfo>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                Button button = buttonMapper.Get(entity);
                MouseInfo mouse = mouseInfoMapper.Get(entity);

                RectangleF hitBox = button.HitBox;

                if (hitBox.Contains(mouse.localPosition))
                {
                    if (mouse.leftButton)
                    {
                        button.OnButtonPress();
                        Debug.WriteLine("UI: " + mouse.localPosition.ToString());
                    }

                    button.OnMouseOver();
                }
                else
                    button.OnMouseExit();
            }
        }
    }
}
