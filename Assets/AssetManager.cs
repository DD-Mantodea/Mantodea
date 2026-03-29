using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea.Assets
{
    public abstract class AssetManager<T>
    {
        public async virtual Task Load()
        {
            GetType().GetFields()
                .Where(t => t.FieldType == typeof(ConcurrentDictionary<string, T>))
                .ToList().ForEach(t =>
                {
                    t.SetValue(this, (ConcurrentDictionary<string, T>)[]);
                });

            GetType().GetFields()
                .Where(t => t.FieldType == typeof(ConcurrentDictionary<string, T>))
                .ToList().ForEach(t =>
                {
                    var a = t.GetValue(this) as ConcurrentDictionary<string, T>;
                    LoadOne(t.Name, a);
                });
        }

        public abstract void LoadOne(string dir, ConcurrentDictionary<string, T> dictronary);
    }
}
