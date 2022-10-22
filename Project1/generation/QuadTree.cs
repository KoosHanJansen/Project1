using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using System;

namespace Project1.rendering
{
    class QuadTree
    {
        private Vector2 position;
        private Transform2 target;
        public RectangleF rect;
        private float size;
        private float halfSize;
        private float cellSize;
        private QuadTree[] branches;
        private Color[,] mapData;
        private int depth;
        
        private World world;
        private Entity chunk;

        private float[] DEPTH_DIST =
        {
            32,
            128,
            256,
            512,
            768,
            1024,
            2048,
            4096,
            8192
        };

        public QuadTree(World world, Vector2 position, Transform2 target, float size, Color[,] mapData, int depth = 8)
        {
            this.position = position;
            this.target = target;
            this.size = size;
            this.cellSize = 32.0f;
            this.rect = new RectangleF(position.X * cellSize, position.Y * cellSize, size * cellSize, size * cellSize);
            this.halfSize = size * 0.5f;
            this.world = world;
            this.mapData = mapData;
            this.depth = depth;

            if (depth == 0)
                CreateChunk();

            UpdateTree();
        }

        private QuadTree GetChunkAt(Vector2 point)
        {
            if (!rect.Contains(point))
                return null;

            if (depth == 0)
                return this;

            if (depth != 0 && branches == null)
                return null;

            for (int i = 0; i < branches.Length; i++)
            {
                if (branches[i].rect.Contains(point))
                    return branches[i].GetChunkAt(point);
            }

            return null;
        }

        public Color GetBlockAt(Vector2 point)
        {
            QuadTree chunk = GetChunkAt(point);

            if (chunk == null)
                return Color.White;

            Vector2 pointInData = new Vector2(MathF.Floor(point.X / cellSize), MathF.Floor(point.Y / cellSize));

            return chunk.mapData[(int)pointInData.Y, (int)pointInData.X];
        }

        public bool PlaceBlockAt(Vector2 point, Color block)
        {
            QuadTree chunk = GetChunkAt(point);

            if (chunk == null)
                return false;

            Vector2 pointInData = new Vector2(MathF.Floor(point.X / cellSize), MathF.Floor(point.Y / cellSize));

            if (chunk.mapData[(int)pointInData.Y, (int)pointInData.X] != Color.White)
                return false;

            chunk.mapData[(int)pointInData.Y, (int)pointInData.X] = block;
            chunk.UpdateChunk();

            return true;
        }

        public bool RemoveBlockAt(Vector2 point, Color block)
        {
            QuadTree chunk = GetChunkAt(point);

            if (chunk == null)
                return false;

            Vector2 pointInData = new Vector2(MathF.Floor(point.X / cellSize), MathF.Floor(point.Y / cellSize));

            if (chunk.mapData[(int)pointInData.Y, (int)pointInData.X] == Color.White)
                return false;

            chunk.mapData[(int)pointInData.Y, (int)pointInData.X] = block;
            chunk.UpdateChunk();

            return true;
        }

        public void UpdateChunk()
        {
            chunk.Destroy();
            CreateChunk();
        }

        public void CreateChunk()
        {
            chunk = world.CreateEntity();
            chunk.Attach(new Transform2(new Vector2((position.X * cellSize) + 256, (position.Y * cellSize) + 256)));
            chunk.Attach(new Sprite(CreateChunkTexture()));
        }

        public void UpdateTree()
        {
            if (depth > 0)
            {
                if (DistanceToTarget() < DEPTH_DIST[this.depth] * cellSize)
                    Subdivide();
                else
                    UnloadBranches();
            }
        }

        private float DistanceToTarget()
        {
            return Vector2.Distance(target.Position, GetCenter());
        }

        private Vector2 GetCenter()
        {
            return new Vector2(rect.Center.X, rect.Center.Y);
        }

        private void Subdivide()
        {
            if (branches != null)
            {
                for (int i = 0; i < branches.Length; i++)
                {
                    branches[i].UpdateTree();
                }

                return;
            }

            branches = new QuadTree[4];

            //Top left
            branches[0] = new QuadTree(world, new Vector2(position.X, position.Y), this.target, halfSize, mapData, this.depth - 1);
            //Top right
            branches[1] = new QuadTree(world, new Vector2(position.X + halfSize, position.Y), this.target, halfSize, mapData, this.depth - 1);
            //Bot left
            branches[2] = new QuadTree(world, new Vector2(position.X, position.Y + halfSize), this.target, halfSize, mapData, this.depth - 1);
            //Bot right
            branches[3] = new QuadTree(world, new Vector2(position.X + halfSize, position.Y + halfSize), this.target, halfSize, mapData, this.depth - 1);
        }

        private Texture2D CreateChunkTexture()
        {
            if (mapData == null)
                return null;

            Texture2D ct = new Texture2D(Game1.graphics.GraphicsDevice, (int)size * (int)cellSize, (int)size * (int)cellSize);
            Color[] data = new Color[ct.Width * ct.Height];

            int index = 0;

            for (int y = 0; y < ct.Height; y++)
            {
                int iy = (int)position.Y + (int)MathF.Floor(y / cellSize);
                for (int x = 0; x < ct.Width; x++)
                {
                    int ix = (int)position.X + (int)MathF.Floor(x / cellSize);

                    if (mapData[iy, ix].Equals(Color.Black))
                        data[index] = Color.White;
                    else if (mapData[iy, ix].Equals(Color.White))
                        data[index] = Color.Transparent;
                    else
                        data[index] = mapData[iy, ix];

                    index++;
                }
            }

            ct.SetData(data);

            return ct;
        }

        private void UnloadBranches()
        {
            if (chunk != null)
            {
                world.DestroyEntity(chunk);
                chunk.Destroy();
            }

            if (branches == null)
                return;
   
            for (int i = 0; i < branches.Length; i++)
            {
                branches[i].UnloadBranches();
            }

            branches = null;
        }
    }
}
