using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;

namespace Project1
{
    class PlayerInputHandler : EntityUpdateSystem
    {
        private ComponentMapper<Velocity> velocityMapper;
        private ComponentMapper<PlayerInput> playerInputMapper;

        private readonly float PLAYER_SPEED;
        private readonly float SPRINT_MULTIPLIER;

        public PlayerInputHandler()
            : base(Aspect.All(typeof(Velocity), typeof(PlayerInput)))
        {
            PLAYER_SPEED = 3;
            SPRINT_MULTIPLIER = 2;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            velocityMapper = mapperService.GetMapper<Velocity>();
            playerInputMapper = mapperService.GetMapper<PlayerInput>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                PlayerInput playerInput = playerInputMapper.Get(entity);
                Velocity velocity = velocityMapper.Get(entity);

                float hor = boolToInt(playerInput.keyRight) + -boolToInt(playerInput.keyLeft);
                float ver = boolToInt(playerInput.keyBackwards) + -boolToInt(playerInput.keyForward);

                velocity.speed.X = hor;
                velocity.speed.Y = ver;

                if (velocity.speed.Length() > 0)
                    velocity.speed.Normalize();

                velocity.speed *= playerInput.Sprint ? PLAYER_SPEED * SPRINT_MULTIPLIER : PLAYER_SPEED;
            }
        }

        private int boolToInt(bool b)
        {
            return Convert.ToInt32(b);
        }
    }
}
