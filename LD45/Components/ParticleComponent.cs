using Artemis.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Components {
    public sealed class ParticleComponent : IComponent {
        public Texture2D Texture { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public float LifeDuration { get; set; }
        public float LifeTimer { get; set; }
        public Func<float, Vector2> ScaleFunction { get; set; } = p => Vector2.One;
        public Color Color { get; set; } = Color.White;
    }
}
