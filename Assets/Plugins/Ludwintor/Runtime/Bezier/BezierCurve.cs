using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    public class BezierCurve : MonoBehaviour
    {
        public Vector3[] points = new Vector3[4];


        public Vector3 GetPoint(float t)
        {
            Vector3 point = LMath.CubicBezier(t, points[0], points[1], points[2], points[3]);
            return transform.TransformPoint(point);
        }

        public Vector3 GetVelocity(float t)
        {
            Vector3 velocity = LMath.CubicBezierDerivative(t, points[0], points[1], points[2], points[3]);
            return transform.TransformPoint(velocity);
        }

        public Vector3 GetDirection(float t) => GetVelocity(t).normalized;

        public void Reset()
        {
            points = new Vector3[4]
            {
                new Vector3(1f, 0f, 0f),
                new Vector3(2f, 0f, 0f),
                new Vector3(3f, 0f, 0f),
                new Vector3(4f, 0f, 0f)
            };
        }
    }
}