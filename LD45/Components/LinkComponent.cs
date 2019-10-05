using Artemis;
using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace LD45.Components {
    public sealed class LinkComponent : IComponent {
        public Entity Parent { get; set; }
        public Vector2 Offset { get; set; } = Vector2.Zero;
    }
}
