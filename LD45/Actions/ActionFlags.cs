using System;

namespace LD45.Actions {
    [Flags]
    public enum ActionFlags {
        None = 0,
        PrefersFar = 1,
        PrefersClose = 2,
        PrefersLowHealth = 4,
    }
}
