using Artemis;
using Artemis.System;
using LD45.Components;

namespace LD45.Systems {
    public sealed class CommanderWeaponSystem : EntityProcessingSystem {
        public CommanderWeaponSystem() 
            : base(Aspect.All(typeof(CommanderComponent), typeof(UnitComponent))) {
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var unitComponent = entity.GetComponent<UnitComponent>();

            if (commanderComponent.Weapon == null) {
                return;
            }

            unitComponent.Action = commanderComponent.Weapon.Action;

            for (int i = 0; i < commanderComponent.Squad.Count; i++) {
                var followerUnitComponent = commanderComponent.Squad[i].GetComponent<UnitComponent>();

                followerUnitComponent.Action = commanderComponent.Weapon.Action;
            }
        }
    }
}
