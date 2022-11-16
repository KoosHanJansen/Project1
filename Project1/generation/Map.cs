using Microsoft.Xna.Framework;
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

        public Vector2 PlayerPosition;

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
            PlayerPosition = Vector2.Zero;
        }

        public unsafe Color[,] LoadMap(string name)
        {
            string path = FileLocations.SAVES_DIRECTORY + "\\" + name + "\\world.dat";

            if (!FileLocations.Exists(path))
                return null;

            Color[,] map = new Color[settings.width, settings.height];

            using (var rs = new FileStream(path, FileMode.Open))
            {
                FileLocations.Read(map, rs);
            }

            SetPlayerPositionSpawnPoint(map);

            return map;
        }

        public unsafe bool SaveMap(string name, Color[,] map)
        {
            string path = FileLocations.SAVES_DIRECTORY + "\\" + name + "\\world.dat";

            Directory.CreateDirectory(FileLocations.SAVES_DIRECTORY + "\\" + name);

            using (var ws = new FileStream(path, FileMode.Create))
            {
                FileLocations.Write(map, ws);
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

            return SetWorldSpawnPoint(data);
        }

        private float SpawnMask(int x, int y, int radius)
        {
            float cX = settings.width * 0.5f;
            float cY = settings.height * 0.5f;

            float distance = Distance(cX, cY, x, y);

            return distance < radius ? 1 : 0;
        }

        private void SetPlayerPositionSpawnPoint(Color[,] data)
        {
            for (int y = 0; y < data.GetLength(0); y++)
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (data[y, x] == Color.Red)
                        PlayerPosition = new Vector2(x * 32, y * 32);
                }
        }

        private Color[,] SetWorldSpawnPoint(Color[,] data)
        {
            int h = settings.height / 2;
            int w = settings.width / 2;

            data[h, w] = Color.Red;
            PlayerPosition = new Vector2(w * 32, h * 32);

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
