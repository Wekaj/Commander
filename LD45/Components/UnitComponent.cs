using Artemis;
using Artemis.Interface;
using LD45.Actions;
using LD45.AI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LD45.Components {
    public sealed class UnitComponent : IComponent {
        public int Team { get; set; }

        public Entity Commander { get; set; } = null;
        public Vector2 Tendency { get; set; }

        public IUnitStrategy Strategy { get; set; } = null;

        public IUnitAction Action { get; set; } = null;
        public float CooldownTimer { get; set; } = 0f;

        public int SightRadius { get; set; } = 100;
        public List<Entity> VisibleUnits { get; } = new List<Entity>();

        public float DistanceWeight { get; set; } = 1f;
    }
}
