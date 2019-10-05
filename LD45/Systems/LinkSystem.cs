using Artemis;
using Artemis.System;
using LD45.Components;

namespace LD45.Systems {
    public sealed class LinkSystem : EntityProcessingSystem {
        private readonly Aspect _transformAspect = Aspect.All(typeof(TransformComponent));

        public LinkSystem() 
            : base(Aspect.All(typeof(LinkComponent), typeof(TransformComponent))) {
        }

        public override void Process(Entity entity) {
            var linkComponent = entity.GetComponent<LinkComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            if (linkComponent.Parent == null) {
                return;
            }

            if (linkComponent.Parent.DeletingState) {
                entity.Delete();
                return;
            }

            if (!_transformAspect.Interests(linkComponent.Parent)) {
                return;
            }

            var parentTransformComponent = linkComponent.Parent.GetComponent<TransformComponent>();

            transformComponent.Position = parentTransformComponent.Position + linkComponent.Offset;
        }
    }
}
