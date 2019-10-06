using Artemis;
using Artemis.Interface;
using LD45.Weapons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LD45.Components {
    public sealed class CommanderComponent : IComponent {
        public List<Vector2> Path { get; } = new List<Vector2>();
        public float AngleOffset { get; set; } = 0f;

        public List<Entity> Squad { get; } = new List<Entity>();

        public IWeapon Weapon { get; set; } = null;

        public Color FlagColor { get; set; } = Color.White;

        public int Strength { get; set; }
        public int Armor { get; set; }
        public int Magic { get; set; }
        public int Resistance { get; set; }
        public int Force { get; set; }
        public int Stability { get; set; }
        public int Range { get; set; }
        public int Speed { get; set; }
        public int Accuracy { get; set; } = 5;
    }
}
