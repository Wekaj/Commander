using LD45.Actions;

namespace LD45.Weapons {
    public sealed class Weapon : IWeapon {
        public IUnitAction Action { get; set; }
    }
}
