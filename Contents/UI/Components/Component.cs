using Mantodea.Contents.DataStructures;
using Mantodea.Contents.Extensions;
using Mantodea.Contents.UI.Components.Containers;
using Mantodea.Contents.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Mantodea.Contents.UI.Components
{
    public class Component
    {
        public Component()
        {
            UserInput.LeftClick += LeftClick;

            UserInput.RightClick += RightClick;

            UserInput.KeepPressLeft += KeepMouseLeft;

            UserInput.KeepPressRight += KeepMouseRight;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public float Alpha { get; set; }

        public float Rotation { get; set; }

        public bool Visible { get; set; }

        public bool HorizontalMiddle { get; set; }

        public bool VerticalMiddle { get; set; }

        public bool Clicked { get; internal set; }

        public bool IsHovering { get; set; }

        public string ID { get; set; }

        public Vector2 Size => new(Width, Height);

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 RelativePosition { get; set; } = Vector2.Zero;

        public Vector2 DrawOffset { get; set; } = Vector2.Zero;

        public Rectangle Rectangle => RectangleUtils.FromVector2(Position, Size);

        public Color BackgroundColor { get; set; }

        public Anchor? Anchor { get; set; }

        public Container? Parent { get; set; }

        public UIEvent OnClickEvent = new();

        public UIEvent OnRightClickEvent = new();

        public UIEvent OnHoverEvent = new();

        public UIEvent OnUpdateEvent = new();

        public virtual void PreDraw(SpriteBatch spriteBatch, GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (BackgroundColor != default)
                spriteBatch.DrawRectangle(new((int)Position.X, (int)Position.Y, Width, Height), BackgroundColor * Alpha);
        }

        public virtual void PostDraw(SpriteBatch spriteBatch, GameTime gameTime) { }

        public virtual void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            PreDraw(spriteBatch, gameTime);

            if (Visible)
                Draw(spriteBatch, gameTime);

            PostDraw(spriteBatch, gameTime);
        }

        public virtual void LeftClick(object sender, int pressTime, Vector2 mouseStart)
        {
            var mouseRect = UserInput.GetMouseRectangle();

            if (mouseRect.Intersects(Rectangle))
            {
                Clicked = true;
                OnClickEvent?.Invoke(this);
            }
        }

        public virtual void RightClick(object sender, int pressTime, Vector2 mouseStart)
        {
            var mouseRect = UserInput.GetMouseRectangle();

            if (mouseRect.Intersects(Rectangle))
            {
                OnRightClickEvent?.Invoke(this);
            }
        }

        public virtual void KeepMouseLeft(object sender, int pressTime, Vector2 mouseStart)
        {

        }

        public virtual void KeepMouseRight(object sender, int pressTime, Vector2 mouseStart)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            OnUpdateEvent?.Invoke(this);

            UpdateMouse(gameTime);

            DrawOffset = new(0, 0);

            Position = RelativePosition + (Parent == null ? new(0, 0) : Parent.Position);
        }

        public virtual void UpdateMouse(GameTime gameTime)
        {
            var mouseRect = UserInput.GetMouseRectangle();

            IsHovering = false;

            Clicked = false;

            if (mouseRect.Intersects(Rectangle))
            {
                IsHovering = true;
                OnHoverEvent?.Invoke(this);
            }
        }

        public void Unload() { }
    }

    public enum Anchor
    {
        Bottom,

        Left,

        Right,

        Top,

        None
    }

    public class UIEvent
    {
        public Dictionary<string, Action<Component>> Listeners = [];

        public void AddListener(string name, Action<Component> listener) => Listeners.TryAdd(name, listener);

        public void RemoveListener(string name) => Listeners.Remove(name);

        public void Invoke(Component c)
        {
            foreach (var listener in Listeners.Values)
                listener.Invoke(c);
        }
    }
}
