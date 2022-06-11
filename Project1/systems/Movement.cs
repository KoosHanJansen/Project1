using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Project1
{
    class Movement : EntityUpdateSystem
    {
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<Velocity> velocityMapper;

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

                if (velocity.speed != null)
                    transform.Position += velocity.speed;
            }
        }
    }
}
