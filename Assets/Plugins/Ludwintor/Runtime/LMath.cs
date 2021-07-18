using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    public static class LMath
    {
        public static Vector3 AngleToVector(float angle)
        {
            float angleRad = angle * Mathf.Deg2Rad;

            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float VectorToAngle(Vector3 dir, bool escapeNegativeAngle = false)
        {
            dir = dir.normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (escapeNegativeAngle && angle < 0)
                angle += 360f;

            return angle;
        }

        public static Vector3 ClampMagnitude(Vector3 vector, float min, float max)
        {
            float sqrMagnitude = vector.sqrMagnitude;
            if (sqrMagnitude > max * max)
                return vector.normalized * max;
            else if (sqrMagnitude < min * min)
                return vector.normalized * min;

            return vector;
        }

        public static Vector2 RotateVector(Vector2 vector, float degrees)
        {
            float angleRad = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            return new Vector2(vector.x * cos - vector.y * sin,
                               vector.x * sin + vector.y * cos);
        }

        public static Vector2 RotateVector90(Vector2 vector, bool clockwise)
        {
            if (clockwise)
                return new Vector2(vector.y, vector.x * -1);
            else
                return new Vector2(vector.y * -1, vector.x);
        }

        /// <summary>
        /// Get point on cubic bezier curve by t value
        /// </summary>
        /// <param name="t">Normalized value (between 0 and 1) to evaluate bezier curve</param>
        /// <param name="p0">First control point (start)</param>
        /// <param name="p1">Second control point</param>
        /// <param name="p2">Third control point</param>
        /// <param name="p3">Fourth control point (end)</param>
        public static Vector3 CubicBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // t can only be between 0 and 1
            t = Mathf.Clamp01(t);
            float tt = t * t;
            float ttt = tt * t;
            float u = 1f - t;
            float uu = u * u;
            float uuu = uu * u;

            // P(t) = (1-t)^3 * p0 + 3t(1-t)^2 * p1 + 3t^2(1-t) * p2 + t^3 * p3 
            return
                uuu * p0 +
                3f * t * uu * p1 +
                3f * tt * u * p2 +
                ttt * p3;
        }

        public static Vector3 CubicBezierDerivative(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            t = Mathf.Clamp01(t);
            float tt = t * t;
            float u = 1f - t;
            float uu = u * u;

            return
                3f * uu * (p1 - p0) +
                6f * u * t * (p2 - p1) +
                3f * tt * (p3 - p2);
        }
    }
}