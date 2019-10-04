using Microsoft.Xna.Framework;
using System;

namespace LD45.Extensions {
    public static class RandomExtensions {
        public static int Next(this Random random, int minValue, int maxValue, params int[] excluding) {
            int range = maxValue - minValue - excluding.Length;

            int value = minValue + random.Next(range);
            for (int i = 0; i < excluding.Length; i++) {
                if (value == excluding[i]) {
                    value++;
                }
            }

            return value;
        }

        public static float NextSingle(this Random random) {
            return (float)random.NextDouble();
        }

        public static float NextSingle(this Random random, float maxValue) {
            return random.NextSingle() * maxValue;
        }

        public static float NextSingle(this Random random, float minValue, float maxValue) {
            return minValue + random.NextSingle(maxValue - minValue);
        }

        public static float NextAngle(this Random random) {
            return random.NextSingle(MathHelper.TwoPi);
        }

        public static float NextAngle(this Random random, float minAngle, float maxAngle) {
            while (minAngle < maxAngle - MathHelper.TwoPi) {
                minAngle += MathHelper.TwoPi;
            }

            while (maxAngle < minAngle) {
                maxAngle += MathHelper.TwoPi;
            }

            return random.NextSingle(minAngle, maxAngle);
        }
    }
}
