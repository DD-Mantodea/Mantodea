using Mantodea.Contents.DataStructures;
using Mantodea.Contents.Extensions;
using Mantodea.Contents.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mantodea.Contents.UI.Components.Containers
{
    public class Container : Component
    {
        public List<Component>? Children { get; set; } = [];

        public Color BorderColor { get; set; }

        public UIVec4 BorderWidth { get; set; } = UIVec4.Zero;

        public bool Scissor { get; set; }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            PreDraw(spriteBatch, gameTime);

            if (Visible)
                DrawSelf(spriteBatch, gameTime);

            if (Scissor)
            {
                var state = spriteBatch.SaveState();

                spriteBatch.EnableScissor();

                spriteBatch.GraphicsDevice.ScissorRectangle = RectangleUtils.FromVector2(Position, Size);

                DrawChildren(spriteBatch, gameTime);

                spriteBatch.LoadState(state);
            }
            else
                DrawChildren(spriteBatch, gameTime);

            PostDraw(spriteBatch, gameTime);
        }

        public virtual void RegisterChild(Component component)
        {
            if (SelectChildById(component.ID) != null && component.ID != "")
                return;
            component.Parent = this;
            Children.Add(component);
        }

        public virtual void RegisterChildAt(int index, Component component, bool behind = false)
        {
            if (SelectChildById(component.ID) != null)
                return;
            if (behind)
                Children.Insert(Children.Count - index, component);
            else
                Children.Insert(index, component);

            SetChildRelativePos(component);
        }

        public bool ContainsChild(Predicate<Component> match)
        {
            return Children.Exists(match);
        }

        public Component SelectChild(Func<Component, bool> match)
        {
            return Children.FirstOrDefault(match, null);
        }

        public T SelectChild<T>(Func<Component, bool> match) where T : Component
        {
            if (Children.FirstOrDefault(match) is not T) return null;
            return Children.FirstOrDefault(match, null) as T;
        }

        public List<Component> SelectChildren(Func<Component, bool> match)
        {
            return Children.Where(match).ToList();
        }

        public T SelectChildById<T>(string id) where T : Component
        {
            if (id == "") return null;
            if (Children.FirstOrDefault(c => c.ID == id, null) is not T) return null;
            else return Children.FirstOrDefault(c => c.ID == id, null) as T;
        }

        public Component SelectChildById(string id)
        {
            if (id == "") return null;
            return Children.FirstOrDefault(c => c.ID == id, null);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            spriteBatch.DrawRectangle(new((int)Position.X, (int)Position.Y, BorderWidth.X, Height), BorderColor);
            spriteBatch.DrawRectangle(new((int)Position.X, (int)Position.Y, Width, BorderWidth.Y), BorderColor);
            spriteBatch.DrawRectangle(new(Width - BorderWidth.Z + (int)Position.X, (int)Position.Y, BorderWidth.Z, Height), BorderColor);
            spriteBatch.DrawRectangle(new((int)Position.X, Height - BorderWidth.W + (int)Position.Y, Width, BorderWidth.W), BorderColor);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateChildren(gameTime);
        }

        public virtual void DrawChildren(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var component in Children)
                component.Draw(spriteBatch, gameTime);
        }

        public virtual void UpdateChildren(GameTime gameTime)
        {
            foreach (var component in Children ?? [])
            {
                if (component.Visible)
                    SetChildRelativePos(component);

                component.Position = component.RelativePosition + Position;

                component.Update(gameTime);
            }
        }

        public virtual void SetChildRelativePos(Component child)
        {
            if (child.HorizontalMiddle)
                child.RelativePosition = new((Width - child.Width) / 2, child.RelativePosition.Y);

            if (child.VerticalMiddle)
                child.RelativePosition = new(child.RelativePosition.X, (Height - child.Height) / 2);

            switch (child.Anchor ?? Components.Anchor.None)
            {
                case Components.Anchor.Left:
                    child.RelativePosition = new(0, child.RelativePosition.Y);
                    break;
                case Components.Anchor.Right:
                    child.RelativePosition = new(Width - child.Width, child.RelativePosition.Y);
                    break;
                case Components.Anchor.Top:
                    child.RelativePosition = new(child.RelativePosition.X, 0);
                    break;
                case Components.Anchor.Bottom:
                    child.RelativePosition = new(child.RelativePosition.X, Height - child.Height);
                    break;
                case Components.Anchor.None:
                    break;
            }
        }
    }
}
