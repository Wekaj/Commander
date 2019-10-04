using Microsoft.Xna.Framework.Input;

namespace LudumDareTemplate.Input {
    public sealed class KeyboardBinding : IBinding {
        public KeyboardBinding(Keys key) {
            Key = key;
        }

        public Keys Key { get; }

        public bool IsPressed(InputState inputState) {
            return inputState.KeyboardState.IsKeyDown(Key);
        }
    }
}
