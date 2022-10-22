using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Project1.rendering;
using System.Runtime.InteropServices;

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
            Vector2 position, unit;

            position = new Vector2(pos.X, pos.Y);
            
            for (int length = 0; length < vel.Length(); length++)
            {
                unit = vel / vel.Length();
                unit = unit * length;

                Vector2 check = position + unit;

                if (SpotFree(check))
                {
                    pos = check;
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
