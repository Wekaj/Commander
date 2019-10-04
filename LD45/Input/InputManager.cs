namespace LD45.Input {
    public sealed class InputManager {
        public InputState State { get; private set; }
        public InputBindings Bindings { get; } = new InputBindings();

        public void Update() {
            State = new InputState();
            Bindings.Update(State);
        }
    }
}
