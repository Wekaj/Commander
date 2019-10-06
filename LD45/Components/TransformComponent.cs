using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace LD45.Components {
    public sealed class TransformComponent : IComponent {
        public Vector2 Position { get; set; }
        public Vector2 Offset { get; set; }
    }
}
