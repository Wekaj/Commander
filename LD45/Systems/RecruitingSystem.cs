using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Systems {
    public sealed class RecruitingSystem : EntityProcessingSystem {
        private const float _recruitingDistance = 16f;

        private readonly Aspect _recruitableAspect = Aspect.All(typeof(RecruitableComponent), typeof(UnitComponent), typeof(BodyComponent));

        public RecruitingSystem() 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent))) {
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            foreach (Entity recruitableEntity in EntityWorld.EntityManager.GetEntities(_recruitableAspect)) {
                var recruitableBodyComponent = recruitableEntity.GetComponent<BodyComponent>();
                var recruitableUnitComponent = recruitableEntity.GetComponent<UnitComponent>();

                float distance = Vector2.Distance(bodyComponent.Position, recruitableBodyComponent.Position);

                if (distance < _recruitingDistance) {
                    commanderComponent.Squad.Add(recruitableEntity);
                    recruitableUnitComponent.Commander = entity;

                    recruitableEntity.RemoveComponent<RecruitableComponent>();
                }
            }
        }
    }
}
