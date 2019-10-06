using Artemis;
using LD45.Components;
using LD45.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Actions {
    public sealed class ThrowBombAction : IUnitAction {
        private readonly EntityBuilder _entityBuilder;
        private readonly EntitySpawner _entitySpawner;

        public ThrowBombAction(IServiceProvider services) {
            _entityBuilder = services.GetRequiredService<EntityBuilder>();
            _entitySpawner = services.GetRequiredService<EntitySpawner>();
        }

        public bool TargetsAllies { get; } = false;
        public ActionAnimation Animation { get; } = ActionAnimation.None;
        public ActionFlags Flags { get; } = ActionFlags.PrefersFar;

        public float Range { get; set; } = 128f;
        public float Cooldown { get; set; } = 5f;

        public void Perform(Entity unit, Entity target) {
            var bodyComponent = unit.GetComponent<BodyComponent>();

            var targetBodyComponent = target.GetComponent<BodyComponent>();

            Vector2 start = bodyComponent.Position;
            Vector2 end = targetBodyComponent.Position;

            _entitySpawner.Enqueue(entityWorld => {
                _entityBuilder.CreateBomb(start, end);
            });
        }
    }
}
