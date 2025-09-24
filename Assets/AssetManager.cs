using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea.Assets
{
    public abstract class AssetManager<T>
    {
        public virtual void Load()
        {
            var dicts = GetType().GetFields().Where(t => t.FieldType == typeof(Dictionary<string, T>)).ToList();

            dicts.ForEach(t => t.SetValue(this, (Dictionary<string, T>)[]));

            dicts.ForEach(t => LoadOneTarget(t.Name, t.GetValue(this) as Dictionary<string, T>));
        }

        public abstract void LoadOneTarget(string dir, Dictionary<string, T> dictronary);
    }
}
