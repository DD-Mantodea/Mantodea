using Mantodea.Contents.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea.Contents.Scenes
{
    public class Scene : GameObject
    {
        public Texture2D Background;

        public Color BackgroundColor = Color.White;

        public List<GameObject> GameObjects = [];

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Background != null)
                spriteBatch.Draw(Background, Vector2.Zero, BackgroundColor);

            foreach (var gameObject in GameObjects)
                gameObject.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var gameObject in GameObjects)
                gameObject.Update(gameTime);
        }
    }
}
