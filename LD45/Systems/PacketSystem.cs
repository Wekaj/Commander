using Artemis;
using Artemis.System;
using LD45.Combat;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Systems {
    public sealed class PacketSystem : EntityProcessingSystem {
        public PacketSystem() 
            : base(Aspect.All(typeof(UnitComponent), typeof(TransformComponent))) {
        }

        public override void Process(Entity entity) {
            var unitComponent = entity.GetComponent<UnitComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();

            for (int i = 0; i < unitComponent.IncomingPackets.Count; i++) {
                Packet packet = unitComponent.IncomingPackets[i];

                unitComponent.Health += packet.HealthChange;

                if (unitComponent.Health > unitComponent.MaxHealth) {
                    unitComponent.Health = unitComponent.MaxHealth;
                }

                Entity indicator = EntityWorld.CreateEntity();
                indicator.AddComponent(new IndicatorComponent {
                    Contents = (packet.HealthChange > 0 ? "+" : "") + packet.HealthChange,
                    Color = packet.HealthChange > 0 ? Color.Green : Color.White,
                });
                indicator.AddComponent(new TransformComponent {
                    Position = transformComponent.Position
                });

                if (unitComponent.Health <= 0) {
                    entity.Delete();
                }
            }

            unitComponent.IncomingPackets.Clear();
        }
    }
}
