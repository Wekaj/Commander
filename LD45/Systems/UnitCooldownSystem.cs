using Artemis;
using Artemis.System;
using LD45.Components;
using System;

namespace LD45.Systems {
    public sealed class UnitCooldownSystem : EntityProcessingSystem {
        private float _deltaTime;

        public UnitCooldownSystem()
            : base(Aspect.All(typeof(UnitComponent))) {
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var unitComponent = entity.GetComponent<UnitComponent>();

            if (unitComponent.CooldownTimer > 0f) {
                unitComponent.CooldownTimer -= _deltaTime;
            }
        }
    }
}
