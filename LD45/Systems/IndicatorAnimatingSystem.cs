using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class IndicatorAnimatingSystem : EntityProcessingSystem {
        private const float _riseSpeed = 30f;
        private const float _maxLife = 1f;

        private float _deltaTime;

        public IndicatorAnimatingSystem()
            : base(Aspect.All(typeof(IndicatorComponent), typeof(TransformComponent))) {
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var indicatorComponent = entity.GetComponent<IndicatorComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            transformComponent.Position -= new Vector2(0f, _riseSpeed) * _deltaTime;

            indicatorComponent.Angle += 10f * _deltaTime;

            float p = indicatorComponent.Life / _maxLife;
            float scale;
            if (p < 0.2f) {
                float subp = p / 0.2f;

                scale = 1f - (float)Math.Pow(1f - subp, 2f);
            }
            else {
                float subp = (p - 0.2f) / 0.8f;

                scale = 1f - (float)Math.Pow(subp, 6f);
            }

            indicatorComponent.Scale = scale;

            indicatorComponent.Life += _deltaTime;
            if (indicatorComponent.Life > _maxLife) {
                entity.Delete();
            }
        }
    }
}
