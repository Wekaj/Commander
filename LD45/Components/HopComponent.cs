using Artemis.Interface;

namespace LD45.Components {
    public sealed class HopComponent : IComponent {
        public bool IsHopping { get; set; }
        public float HopTimer { get; set; }
    }
}
