using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace Project1.libs
{
    public class GameViewport : ViewportAdapter
    {
        private int virtualWidth;
        private int virtualHeight;

        public GameViewport(int vWidth, int vHeight, GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            virtualWidth = vWidth;
            virtualHeight = vHeight;
        }

        public override int VirtualWidth { get { return virtualWidth; } }

        public override int VirtualHeight { get { return virtualHeight; } }

        public override int ViewportWidth { get { return Game1.graphics.GraphicsDevice.Viewport.Width; } }

        public override int ViewportHeight { get { return Game1.graphics.GraphicsDevice.Viewport.Height; } }

        public override Matrix GetScaleMatrix()
        {
            float scaleX = (float)ViewportWidth / VirtualWidth;
            float scaleY = (float)ViewportHeight / VirtualHeight;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }
    }
}
