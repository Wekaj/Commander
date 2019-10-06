using Artemis;
using LD45.Combat;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Actions {
    public sealed class HitAction : IUnitAction {
        private const float _hitForce = 200f;

        public float Range { get; } = 8f;
        public bool TargetsAllies { get; } = false;
        public float Cooldown { get; } = 2f;
        public ActionAnimation Animation { get; } = ActionAnimation.None;

        public void Perform(Entity unit, Entity target) {
            var bodyComponent = unit.GetComponent<BodyComponent>();

            var targetUnitComponent = target.GetComponent<UnitComponent>();
            var targetBodyComponent = target.GetComponent<BodyComponent>();

            Vector2 force = Vector2.Zero;

            float distance = Vector2.Distance(bodyComponent.Position, targetBodyComponent.Position);
            if (distance > 0f) {
                force = (targetBodyComponent.Position - bodyComponent.Position) * _hitForce / distance;
            }

            targetUnitComponent.IncomingPackets.Add(new Packet(unit, -5, DamageType.Physical, force));
        }
    }
}
