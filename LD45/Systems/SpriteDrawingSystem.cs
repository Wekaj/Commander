using Artemis;
using Artemis.System;
using LD45.Components;
using LD45.Graphics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LD45.Systems {
    public sealed class SpriteDrawingSystem : EntityProcessingSystem {
        private readonly Renderer2D _renderer;

        public SpriteDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(SpriteComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();
        }

        public override void Process(Entity entity) {
            var spriteComponent = entity.GetComponent<SpriteComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            _renderer.Draw(spriteComponent.Texture, transformComponent.Position, origin: spriteComponent.Origin);
        }
    }
}
