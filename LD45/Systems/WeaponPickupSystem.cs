using Artemis;
using Artemis.System;
using LD45.Audio;
using LD45.Components;
using LD45.Entities;
using LD45.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Systems {
    public sealed class WeaponPickupSystem : EntityProcessingSystem {
        private const float _pickupDistance = 16f;
        private const float _pickupDistanceSqr = _pickupDistance * _pickupDistance;

        private readonly Aspect _weaponAspect = Aspect.All(typeof(WeaponComponent), typeof(BodyComponent));

        private readonly EntityBuilder _entityBuilder;
        private readonly SoundPlayer _soundPlayer;
        private readonly Random _random;

        private Texture2D _shinyTexture;

        public WeaponPickupSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(CommanderComponent), typeof(BodyComponent))) {

            _entityBuilder = services.GetRequiredService<EntityBuilder>();
            _soundPlayer = services.GetRequiredService<SoundPlayer>();
            _random = services.GetRequiredService<Random>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _shinyTexture = content.Load<Texture2D>("Textures/Shiny");
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            foreach (Entity weaponEntity in EntityWorld.EntityManager.GetEntities(_weaponAspect)) {
                var weaponComponent = weaponEntity.GetComponent<WeaponComponent>();
                var weaponBodyComponent = weaponEntity.GetComponent<BodyComponent>();

                float distanceSqr = Vector2.DistanceSquared(bodyComponent.Position, weaponBodyComponent.Position);

                if (distanceSqr < _pickupDistanceSqr) {
                    if (commanderComponent.Weapon.Icon != null) {
                        Vector2 direction = _random.NextUnitVector();

                        Entity discarded = _entityBuilder.CreateWeapon(bodyComponent.Position + direction * 17f, commanderComponent.Weapon);

                        discarded.GetComponent<BodyComponent>().Impulse += direction * 100f;
                    }

                    commanderComponent.Weapon = weaponComponent.Weapon;

                    CreateShiny(bodyComponent.Position);
                    CreateIconParticle(bodyComponent.Position, weaponComponent.Weapon.Icon);

                    _soundPlayer.Play("Pickup");

                    weaponEntity.Delete();
                    break;
                }
            }
        }

        private void CreateIconParticle(Vector2 position, Texture2D icon) {
            Entity entity = EntityWorld.CreateEntity();
            entity.AddComponent(new ParticleComponent {
                Texture = icon,
                Origin = new Vector2(icon.Width / 2f, icon.Height / 2f),
                LifeDuration = 0.5f,
                ScaleFunction = p => new Vector2(1f - p),
            });
            entity.AddComponent(new TransformComponent {
                Position = position
            });
        }

        private void CreateShiny(Vector2 position) {
            Entity entity = EntityWorld.CreateEntity();
            entity.AddComponent(new ParticleComponent {
                Texture = _shinyTexture,
                Origin = new Vector2(_shinyTexture.Width / 2f, _shinyTexture.Height / 2f),
                LifeDuration = 0.5f,
                ScaleFunction = p => new Vector2(1f - p),
            });
            entity.AddComponent(new TransformComponent {
                Position = position
            });
        }
    }
}
