using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class ProjectileSystem : EntityProcessingSystem {
        private readonly ComponentRemover _remover;

        private float _deltaTime;

        public ProjectileSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(ProjectileComponent), typeof(BodyComponent), typeof(TransformComponent))) {

            _remover = services.GetRequiredService<ComponentRemover>();
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var projectileComponent = entity.GetComponent<ProjectileComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            projectileComponent.DistanceCovered += projectileComponent.Speed * _deltaTime;

            float p = projectileComponent.DistanceCovered / Vector2.Distance(projectileComponent.Start, projectileComponent.End);

            if (p >= 1f) {
                bodyComponent.Position = projectileComponent.End;
                transformComponent.Offset = Vector2.Zero;

                _remover.Remove<ProjectileComponent>(entity);
            }
            else {
                bodyComponent.Position = projectileComponent.Start * (1f - p) + projectileComponent.End * p;

                float height = projectileComponent.HeightFunction(p);

                transformComponent.Offset = new Vector2(0f, -height);
            }
        }
    }
}
