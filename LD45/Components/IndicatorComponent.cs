using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace LD45.Components {
    public sealed class IndicatorComponent : IComponent {
        public string Contents { get; set; } = "";
        public Color Color { get; set; } = Color.White;
        public float Life { get; set; } = 0f;
    }
}
