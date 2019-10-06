using Artemis;
using Artemis.System;
using LD45.Actions;
using LD45.Combat;
using LD45.Components;
using LD45.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Systems {
    public sealed class UnitActionSystem : EntityProcessingSystem {
        private Texture2D _impactTexture, _trailTexture;

        public UnitActionSystem(IServiceProvider services)
            : base(Aspect.All(typeof(UnitComponent), typeof(TransformComponent))) {

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _impactTexture = content.Load<Texture2D>("Textures/Impact");
            _trailTexture = content.Load<Texture2D>("Textures/Trail");
        }

        public override void Process(Entity entity) {
            var unitComponent = entity.GetComponent<UnitComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            if (unitComponent.CooldownTimer > 0f || unitComponent.ActionTarget == null) {
                return;
            }

            unitComponent.Action.Perform(entity, unitComponent.ActionTarget);

            switch (unitComponent.Action.Animation) {
                case ActionAnimation.Projectile: {
                    var targetTransformComponent = unitComponent.ActionTarget.GetComponent<TransformComponent>();

                    float angle = (targetTransformComponent.Position - transformComponent.Position).GetAngle();

                    CreateImpact(targetTransformComponent.Position, angle);
                    CreateTrail(transformComponent.Position, targetTransformComponent.Position);
                    break;
                }
            }

            unitComponent.ActionTarget = null;
            unitComponent.CooldownTimer = CombatEquations.CalculateCooldown(entity);

            if (unitComponent.ActionOrder.Count > 0) {
                unitComponent.ActionOrder.Add(unitComponent.Action);

                unitComponent.Action = unitComponent.ActionOrder[0];
                unitComponent.ActionOrder.RemoveAt(0);
            }
        }

        private void CreateImpact(Vector2 position, float rotation) {
            Entity impact = EntityWorld.CreateEntity();
            impact.AddComponent(new ParticleComponent {
                Texture = _impactTexture,
                Origin = new Vector2(32f, 16f),
                Rotation = rotation,
                LifeDuration = 0.2f,
                ScaleFunction = p => new Vector2(1f - p),
            });
            impact.AddComponent(new TransformComponent {
                Position = position
            });
        }

        private void CreateTrail(Vector2 start, Vector2 end) {
            float angle = (end - start).GetAngle();
            float scale = Vector2.Distance(start, end) / _trailTexture.Width;

            Entity trail = EntityWorld.CreateEntity();
            trail.AddComponent(new ParticleComponent {
                Texture = _trailTexture,
                Origin = new Vector2(0f, 16f),
                Rotation = angle,
                ScaleFunction = p => new Vector2(scale, 1f - p),
                LifeDuration = 0.2f,
            });
            trail.AddComponent(new TransformComponent {
                Position = start
            });
        }
    }
}
