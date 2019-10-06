using Artemis;
using Artemis.System;
using LD45.Components;
using System;

namespace LD45.Systems {
    public sealed class StatAnimatingSystem : EntityProcessingSystem {
        private float _deltaTime;

        public StatAnimatingSystem()
            : base(Aspect.All(typeof(StatDropComponent))) {
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var statComponent = entity.GetComponent<StatDropComponent>();

            statComponent.Angle += 8f * _deltaTime;
            statComponent.SmallAngle -= 6f * _deltaTime;
        }
    }
}
