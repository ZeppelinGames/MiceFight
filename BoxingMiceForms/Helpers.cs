using System;
namespace Editor
{
    public static class Helpers
    {

        public static bool CirclePointIntersect(float x, float y, float cx, float cy, float r)
        {
            return Math.Sqrt(Math.Pow(cx - x, 2) + Math.Pow(cy - y, 2)) < r;
        }

        public static bool CircleCircleIntersect()
        {
            return false;
        }

        public static float Clamp(this float v, float min, float max)
        {
            return Math.Max(Math.Min(v, max), min);
        }

        public static (float, float) Normalise(float x, float y)
        {
            if (x == 0 && y == 0) return (x, y);

            float dist = (float)Math.Sqrt(x * x + y * y);
            return (x / dist, y / dist);
        }
    }
}
