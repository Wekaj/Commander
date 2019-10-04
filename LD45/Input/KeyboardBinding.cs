using Microsoft.Xna.Framework.Input;

namespace LD45.Input {
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
