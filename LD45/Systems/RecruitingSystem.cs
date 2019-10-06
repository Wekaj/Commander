using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class RecruitingSystem : EntityProcessingSystem {
        private const float _recruitingDistance = 16f;

        private readonly Aspect _recruitableAspect = Aspect.All(typeof(RecruitableComponent), typeof(UnitComponent), typeof(BodyComponent));

        private readonly ComponentRemover _componentRemover;

        public RecruitingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent))) {

            _componentRemover = services.GetRequiredService<ComponentRemover>();
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            foreach (Entity recruitableEntity in EntityWorld.EntityManager.GetEntities(_recruitableAspect)) {
                var recruitableComponent = recruitableEntity.GetComponent<RecruitableComponent>();
                var recruitableBodyComponent = recruitableEntity.GetComponent<BodyComponent>();
                var recruitableUnitComponent = recruitableEntity.GetComponent<UnitComponent>();

                float distance = Vector2.Distance(bodyComponent.Position, recruitableBodyComponent.Position);

                if (distance < _recruitingDistance) {
                    if (recruitableComponent.CommanderComponent != null) {
                        _componentRemover.Add(recruitableEntity, recruitableComponent.CommanderComponent);

                        Entity indicator = EntityWorld.CreateEntity();
                        indicator.AddComponent(new IndicatorComponent {
                            Contents = "New Commander!",
                            Color = Color.White,
                        });
                        indicator.AddComponent(new TransformComponent {
                            Position = bodyComponent.Position
                        });
                    }
                    else {
                        if (commanderComponent.Squad.Count == 0) {
                            Entity indicator = EntityWorld.CreateEntity();
                            indicator.AddComponent(new IndicatorComponent {
                                Contents = "Squad Formed!",
                                Color = Color.White,
                            });
                            indicator.AddComponent(new TransformComponent {
                                Position = bodyComponent.Position
                            });
                        }
                        else {
                            Entity indicator = EntityWorld.CreateEntity();
                            indicator.AddComponent(new IndicatorComponent {
                                Contents = "New Recruit!",
                                Color = Color.White,
                            });
                            indicator.AddComponent(new TransformComponent {
                                Position = bodyComponent.Position
                            });
                        }

                        commanderComponent.Squad.Add(recruitableEntity);
                        recruitableUnitComponent.Commander = entity;
                    }

                    recruitableEntity.RemoveComponent<RecruitableComponent>();
                }
            }
        }
    }
}
