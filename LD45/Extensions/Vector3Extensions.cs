using Microsoft.Xna.Framework;

namespace LD45.Extensions {
    public static class Vector3Extensions {
        public static Vector2 Project(this Vector3 vector) {
            return new Vector2(vector.X, vector.Y - vector.Z);
        }

        public static Vector2 Trim(this Vector3 vector) {
            return new Vector2(vector.X, vector.Y);
        }
    }
}
