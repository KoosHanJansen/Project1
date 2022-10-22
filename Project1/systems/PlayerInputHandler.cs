using Microsoft.Xna.Framework;
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

        private readonly float PLAYER_SPEED;
        private readonly float SPRINT_MULTIPLIER;

        private float globalCooldown;

        public PlayerInputHandler()
            : base(Aspect.All(typeof(Velocity), typeof(PlayerInput)))
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

                globalCooldown -= Time.deltaTime;

                if (Game1.mouseInfo.leftButton && globalCooldown < 0)
                {
                    bool blockRemoved = map.RemoveBlockAt(Game1.mouseInfo.position, Color.White);
                    
                    if (blockRemoved)
                    {
                        Debug.WriteLine("Player removed block: " + Game1.mouseInfo.position.ToString());
                        globalCooldown = 0.2f;
                    }
                }

                if (Game1.mouseInfo.rightButton && globalCooldown < 0)
                {
                    bool blockPlaced = map.PlaceBlockAt(Game1.mouseInfo.position, Color.Brown);
                    
                    if (blockPlaced)
                    {
                        Debug.WriteLine("Player placed block: " + Game1.mouseInfo.position.ToString());
                        globalCooldown = 0.2f;
                    }
                }
            }
        }

        private int boolToInt(bool b)
        {
            return Convert.ToInt32(b);
        }
    }
}
