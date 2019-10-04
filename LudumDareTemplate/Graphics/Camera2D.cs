using Microsoft.Xna.Framework;

namespace LudumDareTemplate.Graphics {
    public sealed class Camera2D {
        private Vector2 _position = Vector2.Zero;
        private float _zoom = 1f;
        private float _rotation = 0f;

        private Matrix _transformMatrix = Matrix.Identity;
        private Matrix _inverseMatrix = Matrix.Identity;
        private bool _pendingUpdate = false;

        public Vector2 Position {
            get => _position;
            set {
                _position = value;
                _pendingUpdate = true;
            }
        }

        public float Zoom {
            get => _zoom;
            set {
                _zoom = value;
                _pendingUpdate = true;
            }
        }

        public float Rotation {
            get => _rotation;
            set {
                _rotation = value;
                _pendingUpdate = true;
            }
        }

        public Matrix GetTransformMatrix() {
            UpdateMatrices();

            return _transformMatrix;
        }

        public Matrix GetInverseMatrix() {
            UpdateMatrices();

            return _inverseMatrix;
        }

        public Vector2 ToView(Vector2 worldPosition) {
            return Vector2.Transform(worldPosition, GetTransformMatrix());
        }

        public Vector2 ToWorld(Vector2 viewPosition) {
            return Vector2.Transform(viewPosition, GetInverseMatrix());
        }

        private void UpdateMatrices() {
            if (_pendingUpdate) {
                _transformMatrix = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0f))
                    * Matrix.CreateScale(_zoom)
                    * Matrix.CreateRotationZ(_rotation);
                _inverseMatrix = Matrix.Invert(_transformMatrix);

                _pendingUpdate = false;
            }
        }
    }
}
