using Artemis;
using Artemis.System;
using LD45.Components;
using System;

namespace LD45.Systems {
    public sealed class CommanderAnimatingSystem : EntityProcessingSystem {
        private float _deltaTime;

        public CommanderAnimatingSystem() 
            : base(Aspect.All(typeof(CommanderComponent))) {
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();

            commanderComponent.AngleOffset -= _deltaTime;
        }
    }
}
