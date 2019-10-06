using Artemis;
using LD45.Combat;
using LD45.Components;

namespace LD45.Actions {
    public sealed class HealAction : IUnitAction {
        public float Range { get; } = 64f;
        public bool TargetsAllies { get; } = true;
        public float Cooldown { get; } = 5f;
        public ActionAnimation Animation { get; } = ActionAnimation.None;

        public void Perform(Entity unit, Entity target) {
            var targetUnitComponent = target.GetComponent<UnitComponent>();

            targetUnitComponent.IncomingPackets.Add(Packet.Healing(unit, 10));
        }
    }
}
