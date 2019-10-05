using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LD45.Systems {
    public sealed class SquadMovementSystem : EntityProcessingSystem {
        private const float _passingDistance = 8f;

        public SquadMovementSystem() 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent))) {
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            if (commanderComponent.Path.Count > 0) {
                Vector2 target = commanderComponent.Path[0];

                for (int i = 0; i < commanderComponent.Squad.Count; i++) {
                    Entity followerEntity = commanderComponent.Squad[i];

                    var followerBodyComponent = followerEntity.GetComponent<BodyComponent>();
                    var followerUnitComponent = followerEntity.GetComponent<UnitComponent>();

                    Vector2 followerTarget = target + followerUnitComponent.Tendency;

                    float targetDistance = Vector2.Distance(followerBodyComponent.Position, followerTarget);

                    if (targetDistance > _passingDistance) {
                        followerBodyComponent.Force += Vector2.Normalize(followerTarget - followerBodyComponent.Position) * 150f;
                    }

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
        }

        private Vector2 CalculateRepulsion(BodyComponent bodyComponent1, BodyComponent bodyComponent2) {
            Vector2 difference = bodyComponent1.Position - bodyComponent2.Position;
            float distance = difference.Length();
            if (distance > 0f) {
                float distanceCubed = distance * distance * distance;
                return difference * 150f / distanceCubed;
            }
            else {
                return Vector2.Zero;
            }
        }
    }
}
