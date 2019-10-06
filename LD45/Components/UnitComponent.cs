using Artemis;
using Artemis.Interface;
using LD45.Actions;
using LD45.AI;
using LD45.Combat;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LD45.Components {
    public sealed class UnitComponent : IComponent {
        public int MaxHealth { get; set; }
        public int Health { get; set; }

        public List<Packet> IncomingPackets { get; } = new List<Packet>();

        public int Team { get; set; }

        public Entity Commander { get; set; } = null;
        public Vector2 Tendency { get; set; }

        public IUnitStrategy Strategy { get; set; } = null;

        public IUnitAction Action { get; set; } = null;
        public Entity ActionTarget { get; set; } = null;
        public float CooldownTimer { get; set; } = 0f;

        public List<IUnitAction> ActionOrder { get; } = new List<IUnitAction>();

        public int SightRadius { get; set; } = 100;
        public List<Entity> VisibleUnits { get; } = new List<Entity>();

        public float DistanceWeight { get; set; } = 1f;

        public float StatDropRate { get; set; } = 0f;
    }
}
