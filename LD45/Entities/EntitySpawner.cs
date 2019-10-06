using Artemis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace LD45.Entities {
    public sealed class EntitySpawner {
        private readonly Queue<Action<EntityWorld>> _queue = new Queue<Action<EntityWorld>>();

        private readonly EntityWorld _entityWorld;

        public EntitySpawner(IServiceProvider services) {
            _entityWorld = services.GetRequiredService<EntityWorld>();
        }

        public void Enqueue(Action<EntityWorld> spawnEntity) {
            _queue.Enqueue(spawnEntity);
        }

        public void Spawn() {
            while (_queue.Count > 0) {
                _queue.Dequeue()(_entityWorld);
            }
        }
    }
}
