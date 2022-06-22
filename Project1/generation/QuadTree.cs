using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Project1.rendering
{
    class QuadTree
    {
        private Vector2 position;
        private Vector2 center;
        private Transform2 target;
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
            this.halfSize = size * 0.5f;
            this.world = world;
            this.mapData = mapData;
            this.depth = depth;
            this.cellSize = 32.0f;
            this.center = GetCenter();

            if (depth == 0)
            {
                //Create chunk here
                chunk = world.CreateEntity();   
                chunk.Attach(new Transform2(new Vector2(position.X * cellSize, position.Y * cellSize)));
                chunk.Attach(new Sprite(CreateChunkTexture()));
            }

            UpdateTree();
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
            return Vector2.Distance(target.Position, center);
        }

        private Vector2 GetCenter()
        {
            return new Vector2((position.X + halfSize) * cellSize, (position.Y + halfSize) * cellSize);
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
            Texture2D ct = new Texture2D(Game1.graphics.GraphicsDevice, (int)size * (int)cellSize, (int)size * (int)cellSize);
            Color[] data = new Color[ct.Width * ct.Height];

            int index = 0;

            for (int y = 0; y < ct.Height; y++)
            {
                int iy = (int)position.Y + (int)MathF.Floor(y / cellSize);
                for (int x = 0; x < ct.Width; x++)
                {
                    int ix = (int)position.X + (int)MathF.Floor(x / cellSize);

                    if (mapData[iy, ix].Equals(Color.White))
                        data[index] = Color.Transparent;
                    else if (mapData[iy, ix].Equals(new Color(174, 0, 255)))
                        data[index] = new Color(174, 0, 255);
                    else
                        data[index] = Color.White;

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
