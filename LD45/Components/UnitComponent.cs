using Artemis;
using Artemis.Interface;
using LD45.AI;
using Microsoft.Xna.Framework;

namespace LD45.Components {
    public sealed class UnitComponent : IComponent {
        public IUnitStrategy Strategy { get; set; } = null;
        public Vector2 Tendency { get; set; }
        public Entity Commander { get; set; } = null;
    }
}
