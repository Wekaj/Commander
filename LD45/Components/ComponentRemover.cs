using Artemis;
using Artemis.Interface;
using System;
using System.Collections.Generic;

namespace LD45.Components {
    public sealed class ComponentRemover {
        private readonly Queue<Action> _queue = new Queue<Action>();

        public void Remove<T>(Entity entity)
            where T : IComponent {

            _queue.Enqueue(() => {
                entity.RemoveComponent<T>();
            });
        }

        public void Execute() {
            while (_queue.Count > 0) {
                _queue.Dequeue()();
            }
        }
    }
}
