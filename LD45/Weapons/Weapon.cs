using LD45.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace LD45.Weapons {
    public sealed class Weapon : IWeapon {
        public IUnitAction Action { get; set; }
        public Texture2D Icon { get; set; }
    }
}
