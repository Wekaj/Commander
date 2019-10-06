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
    public sealed class HealthBarDrawingSystem : EntityProcessingSystem {
        private readonly Renderer2D _renderer;

        private Texture2D _pixelTexture;

        public HealthBarDrawingSystem(IServiceProvider services)
            : base(Aspect.All(typeof(UnitComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _pixelTexture = content.Load<Texture2D>("Textures/Pixel");
        }

        public override void Process(Entity entity) {
            var unitComponent = entity.GetComponent<UnitComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            if (unitComponent.Health == unitComponent.MaxHealth || unitComponent.Team != 0 || unitComponent.HealthBarTimer <= 0f) {
                return;
            }

            float p = (float)unitComponent.Health / unitComponent.MaxHealth;

            var color = Color.Lerp(Color.LightPink, Color.Red, 1f - p);

            _renderer.Draw(_pixelTexture, transformComponent.Position - new Vector2(4f, 11f), scale: new Vector2(11f, 1f), color: Color.Lerp(color, Color.Black, 0.75f));
            _renderer.Draw(_pixelTexture, transformComponent.Position - new Vector2(5f, 12f), scale: new Vector2(11f, 1f), color: Color.Lerp(color, Color.Black, 0.75f));
            _renderer.Draw(_pixelTexture, transformComponent.Position - new Vector2(5f, 12f), scale: new Vector2(11f * p, 1f), color: color);
        }
    }
}
