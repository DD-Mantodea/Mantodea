using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea.Contents.UI.Components
{
    public class Timer(int length = 10) : Component
    {
        private int[] _timer = new int[length];

        public int this[int index]
        {
            get => _timer[index];
            set => _timer[index] = value;
        }
    }
}
