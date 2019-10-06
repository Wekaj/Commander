using Artemis;

namespace LD45.Actions {
    public enum ActionAnimation {
        None,
        Projectile,
    }

    public interface IUnitAction {
        float Range { get; }
        bool TargetsAllies { get; }
        float Cooldown { get; }
        ActionAnimation Animation { get; }
        ActionFlags Flags { get; }

        void Perform(Entity unit, Entity target);
    }
}
