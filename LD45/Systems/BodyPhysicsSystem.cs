using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class BodyPhysicsSystem : EntityProcessingSystem {
        private float _deltaTime;

        public BodyPhysicsSystem() 
            : base(Aspect.All(typeof(BodyComponent))) {
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var bodyComponent = entity.GetComponent<BodyComponent>();

            Vector2 totalForce = bodyComponent.Impulse + bodyComponent.Force * _deltaTime;

            Vector2 acceleration = totalForce / bodyComponent.Mass;

            bodyComponent.Velocity += acceleration;

            bodyComponent.Position += bodyComponent.Velocity * _deltaTime;

            bodyComponent.Impulse = Vector2.Zero;
            bodyComponent.Force = Vector2.Zero;

            bodyComponent.Velocity *= (1f - bodyComponent.Friction);
        }
    }
}
