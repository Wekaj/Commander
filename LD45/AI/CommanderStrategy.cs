using Artemis;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.AI {
    public sealed class CommanderStrategy : IUnitStrategy {
        private const float _passingDistance = 12f;

        private readonly Aspect _aspect = Aspect.All(typeof(CommanderComponent), typeof(BodyComponent));

        public void Update(Entity unit) {
            if (!_aspect.Interests(unit)) {
                return;
            }

            var commanderComponent = unit.GetComponent<CommanderComponent>();
            var bodyComponent = unit.GetComponent<BodyComponent>();

            if (commanderComponent.Path.Count > 0) {
                Vector2 target = commanderComponent.Path[0];

                float distance = Vector2.Distance(bodyComponent.Position, target);

                if (distance > _passingDistance) {
                    bodyComponent.Force += Vector2.Normalize(target - bodyComponent.Position) * 150f;
                }
                else if (commanderComponent.Path.Count > 1) {
                    bodyComponent.Force += Vector2.Normalize(target - bodyComponent.Position) * 150f;

                    commanderComponent.Path.RemoveAt(0);
                }
            }
        }
    }
}
