using Artemis;
using Artemis.System;
using LD45.Components;
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

            bodyComponent.Position += bodyComponent.Velocity * _deltaTime;
        }
    }
}
