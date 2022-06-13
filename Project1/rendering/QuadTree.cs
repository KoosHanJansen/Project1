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
        private Transform2 target;
        private float size;
        private QuadTree[] branches;
        
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

        public QuadTree(World world, Vector2 position, Transform2 target, float size, int depth = 8)
        {
            this.position = position;
            this.target = target;
            this.size = size;
            this.world = world;
            this.depth = depth;

            if (depth == 0)
            {
                //Create chunk bullshit here PogChampignon
                /*chunk = world.CreateEntity();   
                chunk.Attach(new Transform2(position));
                chunk.Attach(new Sprite(CreateChunkTexture()));
    
                Debug.WriteLine(position.ToString());*/
            }

            UpdateTree();
        }

        public void UpdateTree()
        {
            if (depth > 0)
            {
                if (DistanceToTarget() < DEPTH_DIST[this.depth])
                    Subdivide();
                else
                    UnloadBranches();
            }
        }

        private float DistanceToTarget()
        {
            return Vector2.Distance(target.Position, position);
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

            float halfSize = this.size * 0.5f;
            branches = new QuadTree[4];

            //Top left
            branches[0] = new QuadTree(world, new Vector2(position.X - halfSize, position.Y + halfSize), this.target, halfSize, this.depth - 1);
            //Top right
            branches[1] = new QuadTree(world, new Vector2(position.X + halfSize, position.Y + halfSize), this.target, halfSize, this.depth - 1);
            //Bot left
            branches[2] = new QuadTree(world, new Vector2(position.X - halfSize, position.Y - halfSize), this.target, halfSize, this.depth - 1);
            //Bot right
            branches[3] = new QuadTree(world, new Vector2(position.X + halfSize, position.Y - halfSize), this.target, halfSize, this.depth - 1);
        }

        private Texture2D CreateChunkTexture()
        {
            Texture2D ct = new Texture2D(Game1.graphics.GraphicsDevice, (int)size * 2, (int)size * 2);
            Color[] data = new Color[ct.Width * ct.Height];

            int index = 0;

            for (int y = 0; y < ct.Height; y++)
            {
                for (int x = 0; x < ct.Width; x++)
                {
                    data[index] = Color.Green;
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
