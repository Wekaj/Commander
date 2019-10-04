using Microsoft.Xna.Framework;
using System;

namespace LD45.Extensions {
    public static class PointExtensions {
        public static Point Clamp(this Point point, Point topLeft, Point bottomRight) {
            return new Point(MathHelper.Clamp(point.X, topLeft.X, bottomRight.X),
                MathHelper.Clamp(point.Y, topLeft.Y, bottomRight.Y));
        }

        public static int ManhattenDistance(this Point point1, Point point2) {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }
    }
}
