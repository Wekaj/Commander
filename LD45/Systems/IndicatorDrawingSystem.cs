using Artemis;
using Artemis.System;
using LD45.Components;
using LD45.Graphics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Systems {
    public sealed class IndicatorDrawingSystem : EntityProcessingSystem {
        private readonly Renderer2D _renderer;

        private SpriteFont _basicFont;

        public IndicatorDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(IndicatorComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _basicFont = content.Load<SpriteFont>("Fonts/Basic");
        }

        public override void Process(Entity entity) {
            var indicatorComponent = entity.GetComponent<IndicatorComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            _renderer.Draw(_basicFont, indicatorComponent.Contents, transformComponent.Position, indicatorComponent.Color, new Vector2(0.5f));
        }
    }
}
