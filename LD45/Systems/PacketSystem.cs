﻿using Artemis;
using Artemis.System;
using LD45.Combat;
using LD45.Components;
using LD45.Entities;
using LD45.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class PacketSystem : EntityProcessingSystem {
        private readonly Aspect _commanderAspect = Aspect.All(typeof(CommanderComponent));
        private readonly Aspect _unitAspect = Aspect.All(typeof(UnitComponent), typeof(BodyComponent));

        private readonly EntityBuilder _entityBuilder;
        private readonly Random _random;

        public PacketSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(UnitComponent), typeof(TransformComponent), typeof(BodyComponent))) {

            _entityBuilder = services.GetRequiredService<EntityBuilder>();
            _random = services.GetRequiredService<Random>();
        }

        public override void Process(Entity entity) {
            var unitComponent = entity.GetComponent<UnitComponent>();
            var transformComponent = entity.GetComponent<TransformComponent>();
            var bodyComponent = entity.GetComponent<BodyComponent>();

            for (int i = 0; i < unitComponent.IncomingPackets.Count; i++) {
                Packet packet = unitComponent.IncomingPackets[i];

                int damage = CombatEquations.CalculateDamage(entity, packet, out bool crit);
                int healing = CombatEquations.CalculateHealing(entity, packet);

                int healthChange = healing - damage;

                unitComponent.Health += healthChange;
                if (unitComponent.Health > unitComponent.MaxHealth) {
                    unitComponent.Health = unitComponent.MaxHealth;
                }

                Vector2 knockback = CombatEquations.CalculateKnockback(entity, packet);

                bodyComponent.Impulse += knockback;

                if (healthChange < 0 && unitComponent.Team != 0 || healthChange > 0) {
                    Entity indicator = EntityWorld.CreateEntity();
                    indicator.AddComponent(new IndicatorComponent {
                        Contents = "" + Math.Abs(healthChange),
                        Color = healthChange > 0 ? Color.Green : (crit ? Color.Lerp(Color.Red, Color.White, 0.5f) : Color.White),
                    });
                    indicator.AddComponent(new TransformComponent {
                        Position = transformComponent.Position
                    });
                }

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

                    commanderComponent.Squad[i].AddComponent(new RecruitableComponent());
                }
            }

            if (_unitAspect.Interests(entity)) {
                var unitComponent = entity.GetComponent<UnitComponent>();
                var bodyComponent = entity.GetComponent<BodyComponent>();

                if (unitComponent.Commander != null) {
                    var commanderComponent = unitComponent.Commander.GetComponent<CommanderComponent>();

                    commanderComponent.Squad.Remove(entity);
                }

                if (_random.NextSingle() < unitComponent.StatDropRate) {
                    _entityBuilder.CreateStatDrop(bodyComponent.Position);
                }
            }

            entity.Delete();
        }
    }
}
