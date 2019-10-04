using Microsoft.Xna.Framework.Input;

namespace LD45.Input {
    public sealed class InputState {
        private readonly GamePadState[] _gamePadStates;

        public InputState(MouseState mouseState, KeyboardState keyboardState, GamePadState[] gamePadStates) {
            _gamePadStates = gamePadStates;

            MouseState = mouseState;
            KeyboardState = keyboardState;
        }

        public InputState() {
            _gamePadStates = new GamePadState[GamePad.MaximumGamePadCount];
            for (int i = 0; i < _gamePadStates.Length; i++) {
                _gamePadStates[i] = GamePad.GetState(i);
            }

            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
        }

        public MouseState MouseState { get; }
        public KeyboardState KeyboardState { get; }

        public GamePadState GetGamePadState(int index) {
            return _gamePadStates[index];
        }
    }
}
