using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea.Contents.Animations
{
    public class Animation<T>(T target, int maxTime = 1, string tag = "") where T : GameObject
    {
        public string Tag = tag;

        public virtual T Target { get; set; } = target;

        public int MaxTime = maxTime;

        public int Time = 0;

        public bool Initialized = false;

        public virtual void Initialize() 
        {
            Initialized = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (MaxTime != 0 && Time == MaxTime) 
                End();
        }

        public virtual void End()
        {
            MaxTime = 0;
        }
    }
}
