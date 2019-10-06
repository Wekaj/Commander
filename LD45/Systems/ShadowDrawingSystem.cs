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
    public sealed class ShadowDrawingSystem : EntityProcessingSystem {
        private readonly Renderer2D _renderer;

        private Texture2D _shadowTexture, _bigShadowTexture;

        public ShadowDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(TransformComponent), typeof(ShadowComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _shadowTexture = content.Load<Texture2D>("Textures/SmallShadow");
            _bigShadowTexture = content.Load<Texture2D>("Textures/BigShadow");
        }

        public override void Process(Entity entity) {
            var transformComponent = entity.GetComponent<TransformComponent>();
            var shadowComponent = entity.GetComponent<ShadowComponent>();

            Texture2D texture = null;
            switch (shadowComponent.Type) {
                case ShadowType.Small: {
                    texture = _shadowTexture;
                    break;
                }
                case ShadowType.Big: {
                    texture = _bigShadowTexture;
                    break;
                }
            }

            _renderer.Draw(texture, transformComponent.Position - transformComponent.Offset, origin: new Vector2(texture.Width / 2f, texture.Height / 2f));
        }
    }
}
