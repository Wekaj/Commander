using Artemis;
using Artemis.System;
using LD45.Components;
using LD45.Graphics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LD45.Systems {
    public sealed class ParticleDrawingSystem : EntityProcessingSystem {
        private readonly Renderer2D _renderer;

        public ParticleDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(ParticleComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();
        }

        public override void Process(Entity entity) {
            var particleComponent = entity.GetComponent<ParticleComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            float p = particleComponent.LifeTimer / particleComponent.LifeDuration;

            _renderer.Draw(particleComponent.Texture, transformComponent.Position, origin: particleComponent.Origin, 
                rotation: particleComponent.Rotation, scale: particleComponent.ScaleFunction(p), color: particleComponent.Color);
        }
    }
}
