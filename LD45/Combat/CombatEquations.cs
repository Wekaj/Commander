using Artemis;
using LD45.Components;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Combat {
    public static class CombatEquations {
        private const float _strengthBonus = 1f / 3f;
        private const float _magicBonus = 1f / 3f;
        private const float _forceBonus = 1f / 3f;
        private const float _speedBonus = 1f / 3f;

        private static readonly CommanderComponent _defaultCommanderComponent = new CommanderComponent();

        public static int CalculateDamage(Entity target, Packet packet) {
            if (packet.HealthChange >= 0) {
                return 0;
            }

            float modifier = 1f;

            var targetCommanderComponent = GetCommanderComponent(target);
            var sourceCommanderComponent = GetCommanderComponent(packet.Source);

            switch (packet.DamageType) {
                case DamageType.Physical: {
                    modifier *= 1f + (sourceCommanderComponent.Strength - targetCommanderComponent.Armor) * _strengthBonus;
                    break;
                }
                case DamageType.Magic: {
                    modifier *= 1f + (sourceCommanderComponent.Magic - targetCommanderComponent.Resistance) * _magicBonus;
                    break;
                }
            }

            modifier = Math.Max(modifier, 0f);

            return (int)Math.Round(-packet.HealthChange * modifier);
        }

        public static int CalculateHealing(Entity target, Packet packet) {
            if (packet.HealthChange <= 0) {
                return 0;
            }

            return packet.HealthChange;
        }

        public static Vector2 CalculateKnockback(Entity target, Packet packet) {
            if (packet.Force == Vector2.Zero) {
                return Vector2.Zero;
            }

            float modifier = 1f;

            var targetCommanderComponent = GetCommanderComponent(target);
            var sourceCommanderComponent = GetCommanderComponent(packet.Source);

            modifier *= 1f + (sourceCommanderComponent.Force - targetCommanderComponent.Stability) * _forceBonus;

            modifier = Math.Max(modifier, 0f);

            return packet.Force * modifier;
        }

        public static float CalculateCooldown(Entity unit) {
            var unitComponent = unit.GetComponent<UnitComponent>();

            if (unitComponent.Action == null) {
                return 1f;
            }

            float modifier = 1f;

            var commanderComponent = GetCommanderComponent(unit);

            modifier *= 1f + commanderComponent.Speed * _speedBonus;

            modifier = Math.Max(modifier, 0.1f);

            return unitComponent.Action.Cooldown / modifier;
        }

        private static CommanderComponent GetCommanderComponent(Entity entity) {
            var unitComponent = entity.GetComponent<UnitComponent>();

            if (unitComponent.Commander != null) {
                return unitComponent.Commander.GetComponent<CommanderComponent>();
            }
            else if (entity.HasComponent<CommanderComponent>()) {
                return entity.GetComponent<CommanderComponent>();
            }
            return _defaultCommanderComponent;
        }
    }
}
