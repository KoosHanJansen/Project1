using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Project1
{
    class PlayerControl : EntityUpdateSystem
    {
        private ComponentMapper<PlayerInput> playerInputMapper;

        public PlayerControl()
            : base(Aspect.All(typeof(Player), typeof(PlayerInput)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            playerInputMapper = mapperService.GetMapper<PlayerInput>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                PlayerInput playerInput = playerInputMapper.Get(entity);

                playerInput.keyForward = Keyboard.GetState().IsKeyDown(Keys.W);
                playerInput.keyLeft = Keyboard.GetState().IsKeyDown(Keys.A);
                playerInput.keyBackwards = Keyboard.GetState().IsKeyDown(Keys.S);
                playerInput.keyRight = Keyboard.GetState().IsKeyDown(Keys.D);
                
                playerInput.Sprint = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
                playerInput.Shoot = Keyboard.GetState().IsKeyDown(Keys.Space);
            }
        }
    }
}
