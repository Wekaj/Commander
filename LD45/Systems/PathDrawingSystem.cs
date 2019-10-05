using Artemis;
using Artemis.System;
using LD45.Components;
using LD45.Extensions;
using LD45.Graphics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Systems {
    public sealed class PathDrawingSystem : EntityProcessingSystem {
        private const float _dotSpacing = 8f;

        private readonly Renderer2D _renderer;

        private Texture2D _dotTexture, _arrowTexture;

        public PathDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(CommanderComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _dotTexture = content.Load<Texture2D>("Textures/Dot");
            _arrowTexture = content.Load<Texture2D>("Textures/Arrow");
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            if (commanderComponent.Path.Count == 0) {
                return;
            }

            float distance = 0f;

            for (int i = commanderComponent.Path.Count - 2; i >= 0; i--) {
                float nodeDistance = Vector2.Distance(commanderComponent.Path[i], commanderComponent.Path[i + 1]);

                Vector2 direction = (commanderComponent.Path[i] - commanderComponent.Path[i + 1]) / nodeDistance;

                while (distance < nodeDistance) {
                    if (distance == 0f) {
                        _renderer.Draw(_arrowTexture, commanderComponent.Path[i + 1] + direction * distance, origin: new Vector2(5f), rotation: (float)Math.PI + direction.GetAngle());
                    }
                    else {
                        _renderer.Draw(_dotTexture, commanderComponent.Path[i + 1] + direction * distance, origin: new Vector2(2f));
                    }

                    distance += _dotSpacing;
                }

                distance -= nodeDistance;
            }

            float finalDistance = Vector2.Distance(transformComponent.Position, commanderComponent.Path[0]);

            Vector2 finalDirection = (transformComponent.Position - commanderComponent.Path[0]) / finalDistance;

            while (distance < finalDistance) {
                _renderer.Draw(_dotTexture, commanderComponent.Path[0] + finalDirection * distance, origin: new Vector2(2f));

                distance += _dotSpacing;
            }
        }
    }
}
