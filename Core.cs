using Mantodea.Contents.UI;
using Microsoft.Xna.Framework;
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

            IsMouseVisible = true;

            Random = new();

            Instance = this;
        }

        public static Core? Instance;

        public static Random? Random;

        public static GraphicsDeviceManager? Graphics;

        public static Matrix GlobalUIScale = Matrix.Identity;

        protected override void Update(GameTime gameTime)
        {
            UserInput.Update();

            base.Update(gameTime);
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
