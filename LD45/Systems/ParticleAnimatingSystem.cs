using Artemis;
using Artemis.System;
using LD45.Components;
using System;

namespace LD45.Systems {
    public sealed class ParticleAnimatingSystem : EntityProcessingSystem {
        private float _deltaTime;

        public ParticleAnimatingSystem()
            : base(Aspect.All(typeof(ParticleComponent), typeof(TransformComponent))) {
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var particleComponent = entity.GetComponent<ParticleComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            particleComponent.LifeTimer += _deltaTime;
            if (particleComponent.LifeTimer >= particleComponent.LifeDuration) {
                entity.Delete();
            }
        }
    }
}
