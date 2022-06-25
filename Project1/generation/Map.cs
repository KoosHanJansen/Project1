using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project1
{
    class Map
    {
        private OpenSimplexNoise noise;
        private Color borderColor;

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

        public Color[,] GenerateMap(MapSettings settings)
        {
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

                    float mask = CenterMask(x, y, settings);
                    
                    float grayScale = (1 + (float)noise.Evaluate((double)noiseX, (double)noiseY)) / 2;
                    
                    if (mask < 0.5f)
                        grayScale *= mask / 0.5f;

                    grayScale = grayScale > settings.density ? 1 : 0;
                    
                    if (IsBorderPixel(x, y, settings))
                        data[y, x] = borderColor;
                    else
                        data[y, x] = new Color(grayScale, grayScale, grayScale);

                    index++;
                }
            }

            return data;
        }

        private float CenterMask(int x, int y, MapSettings settings)
        {
            float cX = settings.width * 0.5f;
            float cY = settings.height * 0.5f;

            float maxDistance = Distance(cX, cY, 0, 0);

            float distance = Distance(cX, cY, x, y);

            return 1 - (distance / maxDistance);
        }

        private bool IsBorderPixel(int x, int y, MapSettings settings)
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
