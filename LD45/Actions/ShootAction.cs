using Artemis;
using LD45.Combat;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Actions {
    public sealed class ShootAction : IUnitAction {
        private const float _hitForce = 100f;

        public float Range { get; } = 64f;
        public bool TargetsAllies { get; } = false;
        public float Cooldown { get; } = 2f;
        public ActionAnimation Animation { get; } = ActionAnimation.Projectile;

        public void Perform(Entity unit, Entity target) {
            var bodyComponent = unit.GetComponent<BodyComponent>();

            var targetUnitComponent = target.GetComponent<UnitComponent>();
            var targetBodyComponent = target.GetComponent<BodyComponent>();

            Vector2 force = Vector2.Zero;

            float distance = Vector2.Distance(bodyComponent.Position, targetBodyComponent.Position);
            if (distance > 0f) {
                force = (targetBodyComponent.Position - bodyComponent.Position) * _hitForce / distance;
            }

            targetUnitComponent.IncomingPackets.Add(new Packet(unit, -3, DamageType.Physical, force));
        }
    }
}
