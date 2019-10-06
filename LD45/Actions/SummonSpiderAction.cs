using Artemis;
using LD45.Components;
using LD45.Entities;
using LD45.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LD45.Actions {
    public sealed class SummonSpiderAction : IUnitAction {
        private readonly EntityBuilder _entityBuilder;
        private readonly EntitySpawner _entitySpawner;
        private readonly Random _random;

        public SummonSpiderAction(IServiceProvider services) {
            _entityBuilder = services.GetRequiredService<EntityBuilder>();
            _entitySpawner = services.GetRequiredService<EntitySpawner>();
            _random = services.GetRequiredService<Random>();
        }

        public float Range { get; } = 128f;
        public bool TargetsAllies { get; } = false;
        public float Cooldown { get; } = 5f;
        public ActionAnimation Animation { get; } = ActionAnimation.None;

        public void Perform(Entity unit, Entity target) {
            var bodyComponent = unit.GetComponent<BodyComponent>();

            _entitySpawner.Enqueue(entityWorld => {
                _entityBuilder.CreateSpider(bodyComponent.Position + _random.NextUnitVector());
            });
        }
    }
}
