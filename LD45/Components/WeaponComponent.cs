using Artemis.Interface;
using LD45.Weapons;

namespace LD45.Components {
    public sealed class WeaponComponent : IComponent {
        public IWeapon Weapon { get; set; }
    }
}
