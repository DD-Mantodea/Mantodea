using Mantodea.Contents.UI;
using Mantodea.Contents.UI.Components;
using Mantodea.Contents.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea.Contents
{
    public abstract class GameObject
    {
        public GameObject() 
        {
            UserInput.LeftClick += LeftClick;

            UserInput.RightClick += RightClick;

            UserInput.KeepPressLeft += KeepMouseLeft;

            UserInput.KeepPressRight += KeepMouseRight;
        }

        public virtual bool Visible { get; set; }

        public bool IsHovering { get; set; }

        public bool Clicked { get; internal set; }

        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

        public virtual float Alpha { get; set; } = 1;

        public virtual float Rotation { get; set; }

        public virtual Vector2 Size => new(Width, Height);

        public virtual Vector2 Position { get; set; } = Vector2.Zero;

        public virtual Vector2 DrawOffset { get; set; } = Vector2.Zero;

        public virtual Rectangle Rectangle => RectangleUtils.FromVector2(Position, Size);

        public UserInputEvent OnClickEvent = new();

        public UserInputEvent OnRightClickEvent = new();

        public UserInputEvent OnHoverEvent = new();

        public UserInputEvent OnUpdateEvent = new();

        public virtual void PreDraw(SpriteBatch spriteBatch, GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }

        public virtual void PostDraw(SpriteBatch spriteBatch, GameTime gameTime) { }

        public virtual void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            PreDraw(spriteBatch, gameTime);

            if (Visible)
                Draw(spriteBatch, gameTime);

            PostDraw(spriteBatch, gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            OnUpdateEvent?.Invoke(this);

            UpdateMouse(gameTime);
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

        public virtual void Unload() { }

        ~GameObject()
        {
            Unload();
        }
    }

    public class UserInputEvent
    {
        public Dictionary<string, Action<GameObject>> Listeners = [];

        public void AddListener(string name, Action<GameObject> listener) => Listeners.TryAdd(name, listener);

        public void RemoveListener(string name) => Listeners.Remove(name);

        public void Invoke(GameObject c)
        {
            foreach (var listener in Listeners.Values)
                listener.Invoke(c);
        }
    }
}
