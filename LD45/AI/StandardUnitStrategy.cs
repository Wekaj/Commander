using Artemis;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.AI {
    public sealed class StandardUnitStrategy : IUnitStrategy {
        private const float _passingDistance = 12f;

        private readonly Aspect _aspect = Aspect.All(typeof(UnitComponent), typeof(BodyComponent));

        public void Update(Entity unit) {
            if (!_aspect.Interests(unit)) {
                return;
            }

            var unitComponent = unit.GetComponent<UnitComponent>();
            var bodyComponent = unit.GetComponent<BodyComponent>();

            if (unitComponent.Commander != null) {
                var commanderComponent = unitComponent.Commander.GetComponent<CommanderComponent>();

                if (commanderComponent.Path.Count > 0) {
                    Vector2 target = commanderComponent.Path[0] + unitComponent.Tendency;

                    float targetDistance = Vector2.Distance(bodyComponent.Position, target);

                    if (targetDistance > _passingDistance) {
                        bodyComponent.Force += Vector2.Normalize(target - bodyComponent.Position) * 150f;
                    }
                }
            }
        }
    }
}
