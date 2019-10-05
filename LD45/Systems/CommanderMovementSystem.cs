using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Systems {
    public sealed class CommanderMovementSystem : EntityProcessingSystem {
        private const float _passingDistance = 8f;

        public CommanderMovementSystem() 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent))) {
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            if (commanderComponent.Path.Count > 0) {
                Vector2 target = commanderComponent.Path[0];

                float distance = Vector2.Distance(bodyComponent.Position, target);

                if (distance > _passingDistance) {
                    bodyComponent.Force += Vector2.Normalize(target - bodyComponent.Position) * 100f;
                }
                else if (commanderComponent.Path.Count > 1) {
                    bodyComponent.Force += Vector2.Normalize(target - bodyComponent.Position) * 100f;

                    commanderComponent.Path.RemoveAt(0);
                }
            }
        }
    }
}
