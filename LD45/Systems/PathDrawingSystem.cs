using Artemis;
using Artemis.System;
using LD45.Components;
using LD45.Extensions;
using LD45.Graphics;
using LD45.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace LD45.Systems {
    public sealed class PathDrawingSystem : EntityProcessingSystem {
        private const float _dotSpacing = 8f;
        private const float _ringRadius = 16f;
        private const float _ringRadiusSqr = _ringRadius * _ringRadius;
        private const int _ringDots = 16;
        private const float _dotAngle = MathHelper.TwoPi / _ringDots;
        private const float _commanderPositionWeight = 2f;

        private readonly Renderer2D _renderer;

        private Texture2D _dotTexture;

        public PathDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(CommanderComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _dotTexture = content.Load<Texture2D>("Textures/Dot");
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            if (commanderComponent.Path.Count == 0) {
                return;
            }

            Vector2 positionSum = transformComponent.Position * _commanderPositionWeight;
            for (int i = 0; i < commanderComponent.Squad.Count; i++) {
                positionSum += commanderComponent.Squad[i].GetComponent<TransformComponent>().Position;
            }
            Vector2 averagePosition = positionSum / (_commanderPositionWeight + commanderComponent.Squad.Count);

            Vector2 start = averagePosition;
            Vector2 end = commanderComponent.Path.Last();

            float angle = commanderComponent.AngleOffset;
            for (int i = 0; i < _ringDots; i++) {
                _renderer.Draw(_dotTexture, end + MathUtilities.VectorFromAngle(angle) * _ringRadius);

                angle += _dotAngle;
            }

            float distance = _ringRadius;

            for (int i = commanderComponent.Path.Count - 2; i >= 0; i--) {
                float nodeDistance = Vector2.Distance(commanderComponent.Path[i], commanderComponent.Path[i + 1]);

                Vector2 direction = (commanderComponent.Path[i] - commanderComponent.Path[i + 1]) / nodeDistance;

                while (distance < nodeDistance) {
                    Vector2 dotPosition = commanderComponent.Path[i + 1] + direction * distance;

                    float endDistanceSqr = Vector2.DistanceSquared(dotPosition, end);

                    if (endDistanceSqr > _ringRadiusSqr) {
                        _renderer.Draw(_dotTexture, dotPosition, origin: new Vector2(2f));
                    }

                    distance += _dotSpacing;
                }

                distance -= nodeDistance;
            }

            float finalDistance = Vector2.Distance(start, commanderComponent.Path[0]);

            Vector2 finalDirection = (start - commanderComponent.Path[0]) / finalDistance;

            while (distance < finalDistance) {
                Vector2 dotPosition = commanderComponent.Path[0] + finalDirection * distance;

                float endDistanceSqr = Vector2.DistanceSquared(dotPosition, end);

                if (endDistanceSqr > _ringRadiusSqr) {
                    _renderer.Draw(_dotTexture, dotPosition, origin: new Vector2(2f));
                }

                distance += _dotSpacing;
            }
        }
    }
}
