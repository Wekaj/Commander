using System;

namespace LD45.Actions {
    public sealed class ActionList {
        public ActionList(IServiceProvider services) {
            SummonSpider = new SummonSpiderAction(services) {
                Range = 128f,
                Cooldown = 5f,
            };
            ThrowBomb = new ThrowBombAction(services) {
                Range = 128f,
                Cooldown = 5f,
            };
        }

        public IUnitAction SummonSpider { get; }
        public IUnitAction ThrowBomb { get; }

        public IUnitAction Punch { get; } = new HitAction {
            Damage = 1,
            Force = 200f,
            Range = 7f,
            Cooldown = 3f,
        };
        public IUnitAction StickSlash { get; } = new HitAction {
            Damage = 3,
            Force = 200f,
            Range = 9f,
            Cooldown = 2.5f,
        };
        public IUnitAction SwordSlash { get; } = new HitAction {
            Damage = 5,
            Force = 200f,
            Range = 8f,
            Cooldown = 2f,
        };
        public IUnitAction BowShoot { get; } = new ShootAction {
            Damage = 3,
            Force = 100f,
            Range = 72f,
            Cooldown = 3f,
        };
        public IUnitAction StaffHeal { get; } = new HealAction();

        public IUnitAction SpiderBite { get; } = new HitAction {
            Damage = 4,
            Force = 200f,
            Range = 8f,
            Cooldown = 2f,
        };
        public IUnitAction SpiderSpit { get; } = new ShootAction {
            Damage = 6,
            Force = 300f,
            Range = 128f,
            Cooldown = 3f,
        };
    }
}
