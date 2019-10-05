using Artemis;

namespace LD45.Combat {
    public sealed class Packet {
        public Packet(Entity source, int healthChange) {
            Source = source;
            HealthChange = healthChange;
        }

        public Entity Source { get; set; }
        public int HealthChange { get; set; }
    }
}
