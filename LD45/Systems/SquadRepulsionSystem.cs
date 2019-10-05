using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Systems {
    public sealed class SquadRepulsionSystem : EntityProcessingSystem {
        private const float _repulsionDistance = 8f;

        public SquadRepulsionSystem() 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent))) {
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            for (int i = 0; i < commanderComponent.Squad.Count; i++) {
                Entity followerEntity = commanderComponent.Squad[i];

                var followerBodyComponent = followerEntity.GetComponent<BodyComponent>();

                Vector2 commanderRepulsionForce = CalculateRepulsion(followerBodyComponent, bodyComponent);

                followerBodyComponent.Force += commanderRepulsionForce;

                for (int j = i + 1; j < commanderComponent.Squad.Count; j++) {
                    Entity followerEntity2 = commanderComponent.Squad[j];

                    var followerBodyComponent2 = followerEntity2.GetComponent<BodyComponent>();

                    Vector2 repulsionForce = CalculateRepulsion(followerBodyComponent, followerBodyComponent2);

                    followerBodyComponent.Force += repulsionForce;
                    followerBodyComponent2.Force -= repulsionForce;
                }
            }
        }

        private Vector2 CalculateRepulsion(BodyComponent bodyComponent1, BodyComponent bodyComponent2) {
            Vector2 difference = bodyComponent1.Position - bodyComponent2.Position;
            float distance = difference.Length();
            if (distance > 0f && distance < _repulsionDistance) {
                return difference * 500f / distance;
            }
            else {
                return Vector2.Zero;
            }
        }
    }
}
