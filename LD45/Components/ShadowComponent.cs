using Artemis.Interface;

namespace LD45.Components {
    public enum ShadowType {
        Small
    }

    public sealed class ShadowComponent : IComponent {
        public ShadowType Type { get; set; }
    }
}
