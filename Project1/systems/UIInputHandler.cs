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
        private ComponentMapper<InputBox> inputBoxMapper;

        public UIInputHandler()
            : base (Aspect.One(typeof(Button), typeof(InputBox)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            buttonMapper = mapperService.GetMapper<Button>();
            inputBoxMapper = mapperService.GetMapper<InputBox>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                Button button = buttonMapper.Get(entity);
                InputBox inputBox = inputBoxMapper.Get(entity);

                if (inputBox != null && !inputBox.active)
                {
                    if (inputBox.HitBox.Contains(Game1.mouseInfo.localPosition))
                    {
                        if (Game1.mouseInfo.leftButton)
                        {
                            inputBox.active = true;
                            Debug.WriteLine("InputBox activated!");
                        }
                    }
                }

                if (button == null || !button.Active)
                    continue;

                RectangleF hitBox = button.HitBox;

                if (hitBox.Contains(Game1.mouseInfo.localPosition))
                {
                    if (Game1.mouseInfo.leftButton)
                    {
                        button.OnButtonPress();
                        Debug.WriteLine("UI: " + Game1.mouseInfo.localPosition.ToString());
                    }

                    button.OnMouseOver();
                }
                else
                    button.OnMouseExit();
            }
        }
    }
}
