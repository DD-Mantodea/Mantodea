using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mantodea.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace Mantodea.Assets.Managers
{
    public class TextureManager : AssetManager<Texture2D>
    {
        public Dictionary<string, Texture2D> Entity = [];

        public Dictionary<string, Texture2D> UI = [];

        public Dictionary<string, Texture2D> Tile = [];

        public Dictionary<string, Dictionary<string, Texture2D>> Assets { get; set; } = [];

        public override void LoadOneTarget(string dir, Dictionary<string, Texture2D> dictronary)
        {
            var path = Path.Combine(Pathes.ContentPath, "Textures", dir);

            if (Directory.Exists(path))
            {
                Directory.GetFiles(path, "*.png", SearchOption.AllDirectories).ToList().ForEach(file =>
                    dictronary?.Add(file.Replace($"{path}\\", "").Replace(".png", ""),
                    Texture2D.FromFile(Core.Instance?.GraphicsDevice, file)));
            }
        }

        public Texture2D this[TexType type, string ID, float scale = 2]
        {
            get
            {
                Texture2D src = null;
                switch (type)
                {
                    case TexType.Entity:
                        src = Entity[ID];
                        break;
                    case TexType.UI:
                        src = UI[ID];
                        break;
                    case TexType.Tile:
                        src = Tile[ID];
                        break;
                }

                return src?.Scale(2);
            }
        }

        public enum TexType : int
        {
            Entity,
            UI,
            Tile
        }
    }
}
