using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace LD45.Components {
    public sealed class BodyComponent : IComponent {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
    }
}
