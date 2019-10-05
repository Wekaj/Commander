using Artemis.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD45.Components {
    public sealed class ParticleComponent : IComponent {
        public Texture2D Texture { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public float LifeDuration { get; set; }
        public float LifeTimer { get; set; }
    }
}
