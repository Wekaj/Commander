using LD45.Extensions;
using System.Collections.Generic;

namespace LD45.Input {
    public sealed class InputBindings {
        private readonly Dictionary<BindingId, IBinding[]> _bindings = new Dictionary<BindingId, IBinding[]>();

        private readonly HashSet<BindingId> _previousBindings = new HashSet<BindingId>();
        private readonly HashSet<BindingId> _pressedBindings = new HashSet<BindingId>();

        public void Set(BindingId id, params IBinding[] bindings) {
            _bindings.Remove(id);
            _bindings.Add(id, bindings);
        }

        public void Update(InputState inputState) {
            _previousBindings.Clear();
            _previousBindings.AddRange(_pressedBindings);

            _pressedBindings.Clear();
            foreach (KeyValuePair<BindingId, IBinding[]> entry in _bindings) {
                for (int i = 0; i < entry.Value.Length; i++) {
                    if (entry.Value[i].IsPressed(inputState)) {
                        _pressedBindings.Add(entry.Key);
                        break;
                    }
                }
            }
        }

        public void Update() {
            Update(new InputState());
        }

        public bool IsPressed(BindingId id) {
            return _pressedBindings.Contains(id);
        }

        public bool WasPressed(BindingId id) {
            return _previousBindings.Contains(id);
        }

        public bool JustPressed(BindingId id) {
            return IsPressed(id) && !WasPressed(id);
        }

        public bool JustReleased(BindingId id) {
            return !IsPressed(id) && WasPressed(id);
        }
    }
}
