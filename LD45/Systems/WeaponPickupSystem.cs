using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;

namespace LD45.Systems {
    public sealed class WeaponPickupSystem : EntityProcessingSystem {
        private const float _pickupDistance = 16f;
        private const float _pickupDistanceSqr = _pickupDistance * _pickupDistance;

        private readonly Aspect _weaponAspect = Aspect.All(typeof(WeaponComponent), typeof(BodyComponent));

        public WeaponPickupSystem() 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent))) {
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            foreach (Entity weaponEntity in EntityWorld.EntityManager.GetEntities(_weaponAspect)) {
                var weaponComponent = weaponEntity.GetComponent<WeaponComponent>();
                var weaponBodyComponent = weaponEntity.GetComponent<BodyComponent>();

                float distanceSqr = Vector2.DistanceSquared(bodyComponent.Position, weaponBodyComponent.Position);

                if (distanceSqr < _pickupDistanceSqr) {
                    commanderComponent.Weapon = weaponComponent.Weapon;

                    weaponEntity.Delete();
                    break;
                }
            }
        }
    }
}
