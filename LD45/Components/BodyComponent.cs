using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace LD45.Components {
    public sealed class BodyComponent : IComponent {
        public float Mass { get; set; } = 1f;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Force { get; set; }
        public Vector2 Impulse { get; set; }
        public float Friction { get; set; } = 2f;
        public float MaxVelocity { get; set; } = 50f;
    }
}
