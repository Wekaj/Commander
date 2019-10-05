using Microsoft.Xna.Framework;
using System;

namespace LD45.Utilities {
    public static class MathUtilities {
        public static Vector2 VectorFromAngle(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
    }
}
