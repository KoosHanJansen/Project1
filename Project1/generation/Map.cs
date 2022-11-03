﻿using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Project1
{
    class Map
    {
        private OpenSimplexNoise noise;
        private Color borderColor;
        private MapSettings settings;

        public MapSettings Settings { set { this.settings = value; } get { return this.settings; } }

        public struct MapSettings {
            public int seed;
            public int width;            
            public int height;
            public int borderSize;
            public float density;
            public float scale;
            public float frequency;
        };

        public Map()
        {    
            borderColor = new Color(174, 0, 255);
        }

        public unsafe Color[,] LoadMap(string name)
        {
            string path = FileLocations.SAVES_DIRECTORY + "\\" + name + ".dat";

            if (!FileLocations.Exists(path))
                return null;

            Color[,] map = new Color[settings.width, settings.height];

            using (var ws = new FileStream(path, FileMode.Open))
            {
                fixed (void* asd = map)
                    ws.Read(new Span<byte>(asd, map.Length * sizeof(uint)));
            }

            return map;
        }

        public unsafe bool SaveMap(string name, Color[,] map)
        {
            string path = FileLocations.SAVES_DIRECTORY + "\\" + name + ".dat";

            using (var ws = new FileStream(path, FileMode.Create))
            {
                fixed (void* asd = map)
                    ws.Write(new Span<byte>(asd, map.Length * sizeof(uint)));
            }
               
            return true;
        }

        public Color[,] GenerateMap()
        {
            if (this.settings.Equals(default(MapSettings)))
                return null;

            noise = new OpenSimplexNoise(settings.seed);

            Color[,] data = new Color[settings.width, settings.height];

            int index = 0;

            float noiseX, noiseY = 0;

            for (int y = 0; y < settings.height; y++)
            {
                noiseY = (y / settings.scale) * settings.frequency;
                for (int x = 0; x < settings.width; x++)
                {
                    noiseX = (x / settings.scale) * settings.frequency;

                    float mask = CenterMask(x, y);
                    
                    float grayScale = (1 + (float)noise.Evaluate((double)noiseX, (double)noiseY)) / 2;
                    
                    if (mask < 0.5f)
                        grayScale *= mask / 0.5f;

                    grayScale = grayScale > settings.density ? 1 : 0;

                    grayScale = SpawnMask(x, y, 25) == 1 ? 1 : grayScale;
                    
                    if (IsBorderPixel(x, y))
                        data[y, x] = borderColor;
                    else
                        data[y, x] = new Color(grayScale, grayScale, grayScale);

                    index++;
                }
            }

            return SetSpawnPoint(data);
        }

        private float SpawnMask(int x, int y, int radius)
        {
            float cX = settings.width * 0.5f;
            float cY = settings.height * 0.5f;

            float distance = Distance(cX, cY, x, y);

            return distance < radius ? 1 : 0;
        }

        private Color[,] SetSpawnPoint(Color[,] data)
        {
            data[settings.height / 2, settings.width / 2] = Color.Red;
            return data;
        }

        private float CenterMask(int x, int y)
        {
            float cX = settings.width * 0.5f;
            float cY = settings.height * 0.5f;

            float maxDistance = Distance(cX, cY, 0, 0);

            float distance = Distance(cX, cY, x, y);

            return 1 - (distance / maxDistance);
        }

        private bool IsBorderPixel(int x, int y)
        {
            if (x < settings.borderSize || x > settings.width - settings.borderSize ||
                y < settings.borderSize || y > settings.height - settings.borderSize)
                return true;

            return false;
        }

        private float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }
    }
}
