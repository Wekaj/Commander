using Artemis;
using Microsoft.Xna.Framework;

namespace LD45.Combat {
    public enum DamageType {
        Pure,
        Physical,
        Magic,
    }

    public sealed class Packet {
        public Packet(Entity source, int healthChange, DamageType damageType, Vector2 force) {
            Source = source;
            HealthChange = healthChange;
            DamageType = damageType;
            Force = force;
        }

        public Entity Source { get; set; }
        public int HealthChange { get; set; }
        public DamageType DamageType { get; set; }
        public Vector2 Force { get; set; }

        public static Packet Healing(Entity source, int amount) {
            return new Packet(source, amount, DamageType.Pure, Vector2.Zero);
        }
    }
}
