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
    public sealed class StatDrawingSystem : EntityProcessingSystem {
        private readonly Renderer2D _renderer;

        private Texture2D _smallSquareTexture, _bigSquareTexture;

        public StatDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(StatDropComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _smallSquareTexture = content.Load<Texture2D>("Textures/StatCentre");
            _bigSquareTexture = content.Load<Texture2D>("Textures/StatBack");
        }

        public override void Process(Entity entity) {
            var statComponent = entity.GetComponent<StatDropComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            Color light = Color.Lerp(statComponent.Color, Color.White, 0.5f);
            Color dark = Color.Lerp(statComponent.Color, Color.White, 0.9f);

            _renderer.Draw(_bigSquareTexture, transformComponent.Position, color: light,
                origin: new Vector2(_bigSquareTexture.Width / 2f, _bigSquareTexture.Height / 2f));
            _renderer.Draw(_smallSquareTexture, transformComponent.Position, color: dark,
                origin: new Vector2(_smallSquareTexture.Width / 2f, _smallSquareTexture.Height / 2f), rotation: statComponent.SmallAngle);
        }
    }
}
