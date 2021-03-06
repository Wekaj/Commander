﻿using Artemis;
using LD45.Combat;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Actions {
    public sealed class MagicAction : IUnitAction {
        public bool TargetsAllies { get; } = false;
        public ActionAnimation Animation { get; } = ActionAnimation.Magic;
        public ActionFlags Flags { get; } = ActionFlags.PrefersClose | ActionFlags.PrefersLowHealth;

        public int Damage { get; set; } = 1;
        public float Force { get; set; } = 100f;
        public float Range { get; set; } = 64f;
        public float Cooldown { get; set; } = 1f;

        public void Perform(Entity unit, Entity target) {
            var bodyComponent = unit.GetComponent<BodyComponent>();

            var targetUnitComponent = target.GetComponent<UnitComponent>();
            var targetBodyComponent = target.GetComponent<BodyComponent>();

            Vector2 force = Vector2.Zero;

            float distance = Vector2.Distance(bodyComponent.Position, targetBodyComponent.Position);
            if (distance > 0f) {
                force = (targetBodyComponent.Position - bodyComponent.Position) * Force / distance;
            }

            targetUnitComponent.IncomingPackets.Add(new Packet(unit, -Damage, DamageType.Magic, force) { Sound = "Zap" });
        }
    }
}
