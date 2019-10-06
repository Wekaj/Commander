using Artemis.Interface;

namespace LD45.Components {
    public sealed class BombComponent : IComponent {
        public float Countdown { get; set; }
        public int Damage { get; set; }
        public float Force { get; set; }
        public float Radius { get; set; }
    }
}
