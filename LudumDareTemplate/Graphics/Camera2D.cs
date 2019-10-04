using Microsoft.Xna.Framework;

namespace Ruut.Graphics {
    public sealed class Camera2D {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Zoom { get; set; } = 1f;
        public float Rotation { get; set; } = 0f;

        public Matrix GetTransformMatrix() {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0f))
                * Matrix.CreateScale(Zoom)
                * Matrix.CreateRotationZ(Rotation);
        }

        public Vector2 ToView(Vector2 worldPosition) {
            return Vector2.Transform(worldPosition, GetTransformMatrix());
        }

        public Vector2 ToWorld(Vector2 viewPosition) {
            return Vector2.Transform(viewPosition, Matrix.Invert(GetTransformMatrix()));
        }
    }
}
