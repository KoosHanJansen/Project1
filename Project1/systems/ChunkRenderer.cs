using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using System;

namespace Project1
{
    class ChunkRenderer : EntityDrawSystem
    {
        private ComponentMapper<Chunk> chunkMapper;

        public ChunkRenderer()
            : base(Aspect.All(typeof(Chunk)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            chunkMapper = mapperService.GetMapper<Chunk>();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                Chunk chunk = chunkMapper.Get(entity);
                
                if (!chunk.isLoaded)
                    LoadChunk(GetEntity(entity), chunk);
            }
        }

        public void LoadChunk(Entity e, Chunk chunk)
        {
            if (chunk.blocks == null)
                return;

            e.Detach<Sprite>();
            e.Detach<Transform2>();

            Texture2D chunkTexture = new Texture2D(Game1.graphics.GraphicsDevice, (int)chunk.Size.X, (int)chunk.Size.Y);
            Color[] data = new Color[chunkTexture.Width * chunkTexture.Height];

            int index = 0;

            for (int y = 0; y < chunkTexture.Height; y++)
            {
                int iy = (int)MathF.Floor(y / chunk.cellSize);
                for (int x = 0; x < chunkTexture.Width; x++)
                {
                    int ix = (int)MathF.Floor(x / chunk.cellSize);

                    if (chunk.blocks[iy, ix].Color.Equals(Color.Black))
                        data[index] = Color.White;
                    else if (chunk.blocks[iy, ix].Color.Equals(Color.White))
                        data[index] = Color.Transparent;
                    else
                        data[index] = chunk.blocks[iy, ix].Color;

                    index++;
                }
            }

            chunkTexture.SetData(data);

            e.Attach(new Transform2(new Vector2(chunk.Position.X, chunk.Position.Y)));
            e.Attach(new Sprite(chunkTexture));

            chunk.isLoaded = true;
        }
    }
}
