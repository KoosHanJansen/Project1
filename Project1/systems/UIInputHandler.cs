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

        public UIInputHandler()
            : base (Aspect.All(typeof(Button)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            buttonMapper = mapperService.GetMapper<Button>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                Button button = buttonMapper.Get(entity);

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
