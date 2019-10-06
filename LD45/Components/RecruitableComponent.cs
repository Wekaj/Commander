using Artemis.Interface;

namespace LD45.Components {
    public sealed class RecruitableComponent : IComponent {
        public CommanderComponent CommanderComponent { get; set; } = null;
    }
}
