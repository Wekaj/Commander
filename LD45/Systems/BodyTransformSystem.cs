using Artemis;
using Artemis.System;
using LD45.Components;

namespace LD45.Systems {
    public sealed class BodyTransformSystem : EntityProcessingSystem {
        public BodyTransformSystem() 
            : base(Aspect.All(typeof(BodyComponent), typeof(TransformComponent))) {
        }

        public override void Process(Entity entity) {
            var bodyComponent = entity.GetComponent<BodyComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            transformComponent.Position = bodyComponent.Position + transformComponent.Offset;
        }
    }
}
