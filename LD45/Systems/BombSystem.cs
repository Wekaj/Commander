using Artemis;
using Artemis.System;
using LD45.Combat;
using LD45.Components;
using LD45.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Systems {
    public sealed class BombSystem : EntityProcessingSystem {
        private readonly Aspect _unitAspect = Aspect.All(typeof(UnitComponent), typeof(BodyComponent));

        private readonly EntityBuilder _entityBuilder;

        private float _deltaTime;

        public BombSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(BombComponent), typeof(TransformComponent), typeof(BodyComponent))) {

            _entityBuilder = services.GetRequiredService<EntityBuilder>();
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var bombComponent = entity.GetComponent<BombComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            bombComponent.Countdown -= _deltaTime;
            if (bombComponent.Countdown <= 0f) {
                float radiusSqr = bombComponent.Radius * bombComponent.Radius;

                foreach (Entity unit in EntityWorld.EntityManager.GetEntities(_unitAspect)) {
                    var unitComponent = unit.GetComponent<UnitComponent>();
                    var unitBodyComponent = unit.GetComponent<BodyComponent>();

                    float distanceSqr = Vector2.DistanceSquared(bodyComponent.Position, unitBodyComponent.Position);

                    if (distanceSqr < radiusSqr) {
                        Vector2 force = Vector2.Zero;
                        if (distanceSqr > 0f) {
                            force = Vector2.Normalize(unitBodyComponent.Position - bodyComponent.Position) * bombComponent.Force;
                        }

                        unitComponent.IncomingPackets.Add(new Packet(null, -bombComponent.Damage, DamageType.Physical, force));
                    }
                }

                _entityBuilder.CreateExplosion(transformComponent.Position);

                entity.Delete();
            }
        }
    }
}
