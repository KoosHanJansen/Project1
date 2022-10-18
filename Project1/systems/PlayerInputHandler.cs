using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Project1.rendering;
using System;
using System.Diagnostics;

namespace Project1
{
    class PlayerInputHandler : EntityUpdateSystem
    {
        private QuadTree map;

        private ComponentMapper<Velocity> velocityMapper;
        private ComponentMapper<PlayerInput> playerInputMapper;
        private ComponentMapper<MouseInfo> mouseInfoMapper;

        private readonly float PLAYER_SPEED;
        private readonly float SPRINT_MULTIPLIER;

        private float digCooldown;

        public PlayerInputHandler()
            : base(Aspect.All(typeof(Velocity), typeof(PlayerInput), typeof(MouseInfo)))
        {
            PLAYER_SPEED = 3;
            SPRINT_MULTIPLIER = 2;
        }

        public void SetMap(QuadTree map)
        {
            this.map = map;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            velocityMapper = mapperService.GetMapper<Velocity>();
            playerInputMapper = mapperService.GetMapper<PlayerInput>();
            mouseInfoMapper = mapperService.GetMapper<MouseInfo>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                PlayerInput playerInput = playerInputMapper.Get(entity);
                Velocity velocity = velocityMapper.Get(entity);
                MouseInfo mouse = mouseInfoMapper.Get(entity);

                float hor = boolToInt(playerInput.keyRight) + -boolToInt(playerInput.keyLeft);
                float ver = boolToInt(playerInput.keyBackwards) + -boolToInt(playerInput.keyForward);

                velocity.speed.X = hor;
                velocity.speed.Y = ver;

                if (velocity.speed.Length() > 0)
                    velocity.speed.Normalize();

                velocity.speed *= playerInput.Sprint ? PLAYER_SPEED * SPRINT_MULTIPLIER : PLAYER_SPEED;

                digCooldown--; 

                if (mouse.leftButton && digCooldown < 0)
                {
                    map.RemoveBlockAt(mouse.position);
                    Debug.WriteLine("Player: " + mouse.position.ToString());
                    digCooldown = 0;
                }
            }
        }

        private int boolToInt(bool b)
        {
            return Convert.ToInt32(b);
        }
    }
}
