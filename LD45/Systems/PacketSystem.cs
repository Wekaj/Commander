using Artemis;
using Artemis.System;
using LD45.Combat;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Systems {
    public sealed class PacketSystem : EntityProcessingSystem {
        private readonly Aspect _commanderAspect = Aspect.All(typeof(CommanderComponent));
        private readonly Aspect _unitAspect = Aspect.All(typeof(UnitComponent));

        public PacketSystem() 
            : base(Aspect.All(typeof(UnitComponent), typeof(TransformComponent), typeof(BodyComponent))) {
        }

        public override void Process(Entity entity) {
            var unitComponent = entity.GetComponent<UnitComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            for (int i = 0; i < unitComponent.IncomingPackets.Count; i++) {
                Packet packet = unitComponent.IncomingPackets[i];

                int damage = CombatEquations.CalculateDamage(entity, packet);
                int healing = CombatEquations.CalculateHealing(entity, packet);

                int healthChange = healing - damage;

                unitComponent.Health += healthChange;
                if (unitComponent.Health > unitComponent.MaxHealth) {
                    unitComponent.Health = unitComponent.MaxHealth;
                }

                Vector2 knockback = CombatEquations.CalculateKnockback(entity, packet);

                bodyComponent.Impulse += knockback;

                Entity indicator = EntityWorld.CreateEntity();
                indicator.AddComponent(new IndicatorComponent {
                    Contents = (healthChange > 0 ? "+" : "") + healthChange,
                    Color = healthChange > 0 ? Color.Green : Color.White,
                });
                indicator.AddComponent(new TransformComponent {
                    Position = transformComponent.Position
                });

                if (unitComponent.Health <= 0) {
                    Kill(entity);
                }
            }

            unitComponent.IncomingPackets.Clear();
        }

        private void Kill(Entity entity) {
            if (_commanderAspect.Interests(entity)) {
                var commanderComponent = entity.GetComponent<CommanderComponent>();

                for (int i = 0; i < commanderComponent.Squad.Count; i++) {
                    var followerUnitComponent = commanderComponent.Squad[i].GetComponent<UnitComponent>();

                    followerUnitComponent.Commander = null;
                }
            }
            else if (_unitAspect.Interests(entity)) {
                var unitComponent = entity.GetComponent<UnitComponent>();

                if (unitComponent.Commander != null) {
                    var commanderComponent = unitComponent.Commander.GetComponent<CommanderComponent>();

                    commanderComponent.Squad.Remove(entity);
                }
            }

            entity.Delete();
        }
    }
}
