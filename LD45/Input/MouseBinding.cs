using LD45.Extensions;

namespace LD45.Input {
    public sealed class MouseBinding : IBinding {
        public MouseBinding(MouseButtons button) {
            Button = button;
        }

        public MouseButtons Button { get; }

        public bool IsPressed(InputState inputState) {
            return inputState.MouseState.IsButtonDown(Button);
        }
    }
}
