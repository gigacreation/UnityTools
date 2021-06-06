using UnityEngine;

namespace GcTools
{
    public static class Vector2Helper
    {
        public static float GetAngle(Vector2 start, Vector2 target)
        {
            Vector2 delta = target - start;
            float degree = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;

            if (degree < 0f)
            {
                degree += 360f;
            }

            return degree;
        }
    }
}
