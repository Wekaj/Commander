using Artemis.Interface;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Components {
    public sealed class ProjectileComponent : IComponent {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
        public float Speed { get; set; }
        public float DistanceCovered { get; set; }
        public Func<float, float> HeightFunction { get; set; } = p => 0f;
    }
}
