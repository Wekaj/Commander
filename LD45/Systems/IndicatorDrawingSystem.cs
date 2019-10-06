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

        private SpriteFont _font;
        private Texture2D _squareTexture;

        public IndicatorDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(IndicatorComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _font = content.Load<SpriteFont>("Fonts/Small");
            _squareTexture = content.Load<Texture2D>("Textures/BigSquare");
        }

        public override void Process(Entity entity) {
            var indicatorComponent = entity.GetComponent<IndicatorComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            //_renderer.Draw(_squareTexture, transformComponent.Position, color: Color.Lerp(Color.LightPink, Color.Black, 0.5f), 
            //    origin: new Vector2(_squareTexture.Width / 2f, _squareTexture.Height / 2f), rotation: indicatorComponent.Angle,
            //    scale: new Vector2(indicatorComponent.Scale));

            _renderer.Draw(_font, indicatorComponent.Contents, transformComponent.Position + new Vector2(1f), Color.Lerp(indicatorComponent.Color, Color.Black, 0.5f),
                scale: indicatorComponent.Scale, origin: new Vector2(0.5f));
            _renderer.Draw(_font, indicatorComponent.Contents, transformComponent.Position, indicatorComponent.Color, 
                scale: indicatorComponent.Scale, origin: new Vector2(0.5f));
        }

        protected override void End() {
            base.End();
        }
    }
}
