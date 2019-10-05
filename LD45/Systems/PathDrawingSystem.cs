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
    public sealed class PathDrawingSystem : EntityProcessingSystem {
        private readonly Renderer2D _renderer;

        private Texture2D _dotTexture;

        public PathDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(CommanderComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _dotTexture = content.Load<Texture2D>("Textures/Dot");
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();

            for (int i = 0; i < commanderComponent.Path.Count; i++) {
                _renderer.Draw(_dotTexture, commanderComponent.Path[i], origin: new Vector2(2f));
            }
        }
    }
}
