﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Chunk
    {
        public Vector2 Position { get; set; }
        public Vector2 LocalPosition { get; set; }
        public Vector2 Size { get; set; }

        public bool isLoaded = false;
        public Block[,] blocks;
        public int cellSize;
    }
}
