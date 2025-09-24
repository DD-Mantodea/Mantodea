using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mantodea.Extensions
{
    public static class Texture2DExt
    {
        public static Texture2D Scale(this Texture2D texture, float scale)
        {
            int newWidth = (int)(texture.Width * scale);

            int newHeight = (int)(texture.Height * scale);

            return texture.ScaleTo(newWidth, newHeight);
        }

        public static Texture2D ScaleTo(this Texture2D texture, int width, int height)
        {
            var device = Core.Instance?.GraphicsDevice;

            var originalTargets = device?.GetRenderTargets();

            var render = new RenderTarget2D(device, width, height);

            device?.SetRenderTarget(render);

            device?.Clear(Color.Transparent);

            var spriteBatch = new SpriteBatch(device);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointWrap,
                DepthStencilState.None,
                RasterizerState.CullNone
            );

            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);

            spriteBatch.End();

            device?.SetRenderTargets(originalTargets);

            var scaled = new Texture2D(device, width, height);

            Color[] data = new Color[width * height];

            render.GetData(data);

            scaled.SetData(data);

            render.Dispose();

            return scaled;
        }

        public static Texture2D[] SplitIntoTextures(this Texture2D texture, int tileWidth, int tileHeight)
        {
            List<Rectangle> sourceRects = [];

            int tileCountX = texture.Width / tileWidth;

            int tileCountY = texture.Height / tileHeight;

            for (int y = 0; y < tileCountY; y++)
            {
                for (int x = 0; x < tileCountX; x++)
                {
                    Rectangle rect = new Rectangle(
                        x * tileWidth,
                        y * tileHeight,
                        tileWidth,
                        tileHeight
                    );

                    sourceRects.Add(rect);
                }
            }

            Texture2D[] textures = new Texture2D[sourceRects.Count];

            for (int i = 0; i < sourceRects.Count; i++)
            {
                Rectangle rect = sourceRects[i];

                Texture2D tile = new(Core.Instance?.GraphicsDevice, rect.Width, rect.Height);

                Color[] data = new Color[rect.Width * rect.Height];

                texture.GetData(0, rect, data, 0, data.Length);

                tile.SetData(data);

                textures[i] = tile;
            }

            return textures;
        }

        public static Texture2D[] HorizontalSplit(this Texture2D texture, int number)
        {
            return texture.SplitIntoTextures(texture.Width, texture.Height / number);
        }

        public static Texture2D[] VerticalSplit(this Texture2D texture, int number)
        {
            return texture.SplitIntoTextures(texture.Width / number, texture.Height);
        }
    }
}
