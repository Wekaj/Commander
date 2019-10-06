using Artemis;
using Artemis.System;
using LD45.Audio;
using LD45.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class StatPickupSystem : EntityProcessingSystem {
        private const float _pickupDistance = 16f;
        private const float _pickupDistanceSqr = _pickupDistance * _pickupDistance;

        private readonly Aspect _statAspect = Aspect.All(typeof(StatDropComponent), typeof(BodyComponent));

        private readonly SoundPlayer _soundPlayer;

        public StatPickupSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent), typeof(TransformComponent))) {

            _soundPlayer = services.GetRequiredService<SoundPlayer>();
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            foreach (Entity statEntity in EntityWorld.EntityManager.GetEntities(_statAspect)) {
                var statComponent = statEntity.GetComponent<StatDropComponent>();
                var statBodyComponent = statEntity.GetComponent<BodyComponent>();

                float distanceSqr = Vector2.DistanceSquared(bodyComponent.Position, statBodyComponent.Position);

                if (distanceSqr < _pickupDistanceSqr) {
                    commanderComponent.Strength += statComponent.Strength;
                    commanderComponent.Armor += statComponent.Armor;
                    commanderComponent.Magic += statComponent.Magic;
                    commanderComponent.Resistance += statComponent.Resistance;
                    commanderComponent.Force += statComponent.Force;
                    commanderComponent.Stability += statComponent.Stability;
                    commanderComponent.Range += statComponent.Range;
                    commanderComponent.Speed += statComponent.Speed;
                    commanderComponent.Accuracy += statComponent.Accuracy;

                    Entity indicator = EntityWorld.CreateEntity();
                    indicator.AddComponent(new IndicatorComponent {
                        Contents = statComponent.Message,
                        Color = Color.Lerp(statComponent.Color, Color.White, 0.75f),
                    });
                    indicator.AddComponent(new TransformComponent {
                        Position = transformComponent.Position
                    });

                    _soundPlayer.Play("Powerup");

                    statEntity.Delete();
                    break;
                }
            }
        }
    }
}
