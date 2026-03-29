using Mantodea.Contents.Extensions;
using Mantodea.Contents.UI.Components.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mantodea.Contents.UI.Components
{
    public abstract class Component : GameObject
    {
        public bool HorizontalMiddle { get; set; }

        public bool VerticalMiddle { get; set; }

        public string ID { get; set; }

        public Vector2 RelativePosition { get; set; } = Vector2.Zero;

        public Color BackgroundColor { get; set; }

        public Anchor? Anchor { get; set; }

        public Container? Parent { get; set; }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (BackgroundColor != default)
                spriteBatch.DrawRectangle(new((int)Position.X, (int)Position.Y, Width, Height), BackgroundColor * Alpha);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DrawOffset = new(0, 0);

            Position = RelativePosition + (Parent == null ? new(0, 0) : Parent.Position);
        }
    }

    public enum Anchor
    {
        Bottom,

        Left,

        Right,

        Top,

        None
    }
}
