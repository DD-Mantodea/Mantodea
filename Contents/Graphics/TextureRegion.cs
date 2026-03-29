using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mantodea.Contents.Graphics
{
    public class TextureRegion
    {
        public TextureRegion() { }

        public TextureRegion(Texture2D texture, int x, int y, int width, int height)
        {
            Texture = texture;

            SourceRectangle = new(x, y, width, height);
        }

        public Texture2D Texture { get; set; }

        public Rectangle SourceRectangle { get; set; }

        public virtual int Width => SourceRectangle.Width;

        public virtual int Height => SourceRectangle.Height;

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            Draw(spriteBatch, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            Draw(
                spriteBatch,
                position,
                color,
                rotation,
                origin,
                new Vector2(scale, scale),
                effects,
                layerDepth
            );
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(
                Texture,
                position,
                SourceRectangle,
                color,
                rotation,
                origin,
                scale,
                effects,
                layerDepth
            );
        }
    }
}
