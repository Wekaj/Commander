using Artemis;
using Artemis.System;
using LD45.Audio;
using LD45.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class HopSystem : EntityProcessingSystem {
        private const float _hopTime = 0.2f;
        private const float _hopHeight = 3f;

        private readonly SoundPlayer _soundPlayer;

        private float _deltaTime;

        public HopSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(HopComponent), typeof(TransformComponent))) {

            _soundPlayer = services.GetRequiredService<SoundPlayer>();
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var hopComponent = entity.GetComponent<HopComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            if (hopComponent.IsHopping) {
                hopComponent.HopTimer += _deltaTime;

                if (hopComponent.HopTimer >= _hopTime) {
                    transformComponent.Offset = Vector2.Zero;

                    hopComponent.HopTimer -= _hopTime;

                    hopComponent.IsHopping = false;
                }
                else {
                    float p = hopComponent.HopTimer / _hopTime;

                    float height = (float)Math.Sin(p * Math.PI) * _hopHeight;

                    transformComponent.Offset = new Vector2(0f, -height);
                }
            }
        }
    }
}
