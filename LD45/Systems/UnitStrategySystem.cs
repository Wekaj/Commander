using Artemis;
using Artemis.System;
using LD45.Components;

namespace LD45.Systems {
    public sealed class UnitStrategySystem : EntityProcessingSystem {
        public UnitStrategySystem() 
            : base(Aspect.All(typeof(UnitComponent))) {
        }

        public override void Process(Entity entity) {
            var unitComponent = entity.GetComponent<UnitComponent>();

            unitComponent.Strategy?.Update(entity);
        }
    }
}
