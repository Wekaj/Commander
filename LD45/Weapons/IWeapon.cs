using LD45.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace LD45.Weapons {
    public interface IWeapon {
        IUnitAction Action { get; }
        Texture2D Icon { get; }
    }
}
