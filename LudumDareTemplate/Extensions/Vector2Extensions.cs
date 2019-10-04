using Microsoft.Xna.Framework;
using System;

namespace LudumDareTemplate.Extensions {
    public static class Vector2Extensions {
        public static Vector2 Floor(this Vector2 vector) {
            return new Vector2((float)Math.Floor(vector.X), (float)Math.Floor(vector.Y));
        }

        public static float GetAngle(this Vector2 vector) {
            return ((float)Math.Atan2(vector.Y, vector.X) + MathHelper.TwoPi) % MathHelper.TwoPi;
        }
    }
}