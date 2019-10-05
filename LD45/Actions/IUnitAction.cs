using Artemis;

namespace LD45.Actions {
    public interface IUnitAction {
        float Range { get; }
        bool TargetsAllies { get; }
        float Cooldown { get; }

        void Perform(Entity unit, Entity target);
    }
}
