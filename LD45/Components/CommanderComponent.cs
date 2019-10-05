using Artemis;
using Artemis.Interface;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LD45.Components {
    public sealed class CommanderComponent : IComponent {
        public List<Vector2> Path { get; } = new List<Vector2>();
        public float AngleOffset { get; set; } = 0f;

        public List<Entity> Squad { get; } = new List<Entity>();
    }
}
