using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
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
            Chunk c = chunk.Get<Chunk>();
            c.isLoaded = false;
            c.blocks = GetChunkBlocks();
        }

        public void CreateChunk()
        {
            chunk = world.CreateEntity();

            Chunk c = new Chunk();

            c.Position = new Vector2((position.X * cellSize) + 256, (position.Y * cellSize) + 256);
            c.LocalPosition = new Vector2(position.X, position.Y);
            c.Size = new Vector2((int)size * (int)cellSize, (int)size * (int)cellSize);
            c.blocks = GetChunkBlocks();
            c.cellSize = (int)cellSize;

            chunk.Attach(c);
        }

        private Block[,] GetChunkBlocks()
        {
            Block[,] result = new Block[(int)size, (int)size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Block block = new Block();
                    block.Position = new Vector2(x, y);
                    block.LocalPosition = new Vector2(y, x);
                    block.Color = mapData[(int)position.Y + y, (int)position.X + x];

                    result[y, x] = block;
                }
            }

            return result;
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
