using FontStashSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea.Assets.Managers
{
    public class FontManager : AssetManager<FontSystem>
    {
        public Dictionary<string, FontSystem> Fonts = [];

        public Dictionary<string, Dictionary<string, FontSystem>> Assets { get; set; } = [];

        public override void LoadOneTarget(string dir, Dictionary<string, FontSystem> dictronary)
        {
            if (dictronary == null)
                return;

            var path = Path.Combine(Pathes.ContentPath, "Fonts", dir);

            if (Directory.Exists(path))
            {
                Directory.GetFiles(path, "*.ttf").ToList().ForEach(file =>
                {
                    var font = new FontSystem();

                    font.AddFont(File.ReadAllBytes(file));

                    dictronary.Add(Path.GetFileNameWithoutExtension(file), font);
                });
            }
        }

        public SpriteFontBase this[string id, float size = 20]
        {
            get => Fonts[id].GetFont(size);
        }
    }
}
