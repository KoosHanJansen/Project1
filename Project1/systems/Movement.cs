using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Project1.rendering;

namespace Project1
{
    class Movement : EntityUpdateSystem
    {
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<Velocity> velocityMapper;

        private QuadTree map;

        public Movement()
            : base(Aspect.All(typeof(Transform2), typeof(Velocity)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            velocityMapper = mapperService.GetMapper<Velocity>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                Transform2 transform = transformMapper.Get(entity);
                Velocity velocity = velocityMapper.Get(entity);

                if (velocity.speed == default(Vector2))
                    continue;

                transform.Position = ContinuousCollisionDetection(transform.Position, velocity.speed);
            }
        }

        public void SetMap(QuadTree map)
        {
            this.map = map;
        }

        private Vector2 ContinuousCollisionDetection(Vector2 pos, Vector2 vel)
        {
            Vector2 unit;

            for (int length = 0; length < vel.Length(); length++)
            {
                unit = vel / vel.Length();

                Vector2 check = pos + unit;

                if (SpotFree(check))
                {
                    pos += unit;
                } 
                else if (SpotFree(new Vector2(pos.X + unit.X, pos.Y)))
                {
                    pos.X += unit.X;
                }
                else if (SpotFree(new Vector2(pos.X, pos.Y + unit.Y)))
                {
                    pos.Y += unit.Y;
                }
            }

            return pos;
        }

        private bool SpotFree(Vector2 check)
        {
            if (map == null) return false;

            return map.GetBlockAt(check) == Color.White;
        }
    }
}
