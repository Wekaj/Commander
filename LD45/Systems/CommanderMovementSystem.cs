using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class CommanderMovementSystem : EntityProcessingSystem {
        private const float _passingDistance = 1f;

        private float _deltaTime;

        public CommanderMovementSystem() 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent))) {
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            if (commanderComponent.Path.Count > 0) {
                Vector2 target = commanderComponent.Path[0];

                bodyComponent.Velocity = Vector2.Normalize(target - bodyComponent.Position) * 50f;

                float distance = Vector2.Distance(bodyComponent.Position, target);

                if (distance < _passingDistance) {
                    commanderComponent.Path.RemoveAt(0);
                }
            }
            else {
                bodyComponent.Velocity = Vector2.Zero;
            }
        }
    }
}
