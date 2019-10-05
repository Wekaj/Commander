using Artemis;
using Artemis.System;
using LD45.Components;
using LD45.Extensions;
using LD45.Graphics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace LD45.Systems {
    public sealed class SpriteDrawingSystem : EntityProcessingSystem {
        private readonly Renderer2D _renderer;

        private readonly SortedSet<Entity> _sortedEntities;

        public SpriteDrawingSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(SpriteComponent), typeof(TransformComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            _sortedEntities = new SortedSet<Entity>(Comparer<Entity>.Create(CompareEntities));
        }

        protected override void ProcessEntities(IDictionary<int, Entity> entities) {
            _sortedEntities.AddRange(entities.Values);

            foreach (Entity entity in _sortedEntities) {
                Process(entity);
            }
        }

        public override void Process(Entity entity) {
            var spriteComponent = entity.GetComponent<SpriteComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            _renderer.Draw(spriteComponent.Texture, transformComponent.Position, origin: spriteComponent.Origin);
        }

        protected override void End() {
            base.End();

            _sortedEntities.Clear();
        }

        private int CompareEntities(Entity entity1, Entity entity2) {
            var transformComponent1 = entity1.GetComponent<TransformComponent>();
            var transformComponent2 = entity2.GetComponent<TransformComponent>();

            return transformComponent1.Position.Y.CompareTo(transformComponent2.Position.Y);
        }
    }
}
