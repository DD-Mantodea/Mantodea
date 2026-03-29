using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantodea.Contents.Animations
{
    public class AnimationChain<T> : Animation<T> where T : GameObject
    {
        public AnimationChain(T target, string tag = "", params Animation<T>[] animations) : base(target, int.MaxValue, tag)
        {
            _animationIndex = 0;

            Animations = [.. animations];
        }

        public override T Target
        {
            get => base.Target;
            set
            {
                base.Target = value;
                if (value == null) return;
                foreach (var i in Animations)
                    i.Target = value;
            }
        }

        public bool ShouldBreak;

        public List<Animation<T>> Animations;

        protected int _animationIndex;

        public void RegisterAnimation(Animation<T> animation)
        {
            animation.Target = Target;

            Animations.Add(animation);
        }
    }
}
