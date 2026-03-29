using Mantodea.Contents.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea
{
    public class Core : Game
    {
        public Core(int gameWidth, int gameHeight)
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = gameWidth,
                PreferredBackBufferHeight = gameHeight
            };

            GameHeight = gameHeight;

            GameWidth = gameWidth;

            IsMouseVisible = true;

            Random = new();

            Instance = this;
        }

        public static Core? Instance;

        public static Random? Random;

        public static GraphicsDeviceManager? Graphics;

        public static SpriteBatch SpriteBatch;

        public static int GameHeight { get; set; }

        public static int GameWidth { get; set; }

        public static Vector2 GameSize => new(GameWidth, GameHeight);

        protected override void Update(GameTime gameTime)
        {
            UserInput.Update();

            base.Update(gameTime);
        }

        protected override void Initialize()
        {
            UserInput.Initialize();

            SpriteBatch = new(GraphicsDevice);

            base.Initialize();
        }
    }

    public class Pathes
    {
        public static string GamePath => Environment.CurrentDirectory;

        public static string ContentPath => Path.Combine(GamePath, "Contents");

        public static string ModPath => Path.Combine(GamePath, "Mods");

        public static string ModSourcePath => Path.Combine(GamePath, "ModSources");
    }
}
