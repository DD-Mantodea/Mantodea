using Microsoft.Xna.Framework;
using Mantodea.Contents.UI.Components;
using Mantodea.Contents.UI.Components.Containers;

namespace Mantodea.Contents.Extensions
{
    public static class ComponentExt
    {
        public static T Join<T>(this T component, Container container) where T : Component
        {
            container.RegisterChild(component);

            return component;
        }

        public static T SetRelativePos<T>(this T component, Vector2 relativePos) where T : Component
        {
            component.RelativePosition = relativePos;

            return component;
        }
    }
}
