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

        public void Perform(Entity unit, Entity target) {
            var unitComponent = unit.GetComponent<UnitComponent>();
            var bodyComponent = unit.GetComponent<BodyComponent>();

            var targetUnitComponent = target.GetComponent<UnitComponent>();
            var targetBodyComponent = target.GetComponent<BodyComponent>();

            targetUnitComponent.IncomingPackets.Add(new Packet(unit, -5));

            float distance = Vector2.Distance(bodyComponent.Position, targetBodyComponent.Position);
            if (distance > 0f) {
                targetBodyComponent.Impulse += (targetBodyComponent.Position - bodyComponent.Position) * _hitForce / distance;
            }
        }
    }
}
