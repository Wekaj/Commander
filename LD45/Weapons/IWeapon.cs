using LD45.Actions;

namespace LD45.Weapons {
    public interface IWeapon {
        IUnitAction Action { get; }
    }
}
