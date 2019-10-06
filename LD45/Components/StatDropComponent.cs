using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace LD45.Components {
    public sealed class StatDropComponent : IComponent {
        public Color Color { get; set; }
        public string Message { get; set; }

        public float Angle { get; set; }
        public float SmallAngle { get; set; }

        public int Strength { get; set; }
        public int Armor { get; set; }
        public int Magic { get; set; }
        public int Resistance { get; set; }
        public int Force { get; set; }
        public int Stability { get; set; }
        public int Range { get; set; }
        public int Speed { get; set; }
        public int Accuracy { get; set; }
    }
}
