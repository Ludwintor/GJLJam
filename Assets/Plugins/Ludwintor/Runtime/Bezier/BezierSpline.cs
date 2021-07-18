using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ludwintor.Tools
{
    public class BezierSpline : MonoBehaviour
    {
        [SerializeField] private List<Vector3> points;
        [SerializeField] private BezierControlPointMode[] modes;
        [SerializeField] private bool loop;

        public List<Vector3> Points => points;
        public List<Vector3> ControlPoints => points.Skip(1).ToList();
        public int CurveCount => (points.Count - 1) / 3;
        public bool Loop 
        { 
            get => loop; 
            set
            {
                loop = value;
                if (value)
                {
                    modes[modes.Length - 1] = modes[0];
                    SetControlPoint(0, points[0]);
                }
            }
        }

        public Vector3 GetPoint(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Count - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }

            Vector3 point = LMath.CubicBezier(t, points[i], points[i + 1], points[i + 2], points[i + 3]);
            return transform.TransformPoint(point);
        }

        public Vector3 GetVelocity(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Count - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }

            Vector3 velocity = LMath.CubicBezierDerivative(t, points[i], points[i + 1], points[i + 2], points[i + 3]);
            return transform.TransformPoint(velocity);
        }

        public Vector3 GetDirection(float t) => GetVelocity(t).normalized;

        public void AddCurve()
        {
            Vector3 point = points[points.Count - 1];
            points.Capacity += 3;
            point.x += 5f;
            points.Add(point);
            point.x += 5f;
            points.Add(point);
            point.x += 5f;
            points.Add(point);

            Array.Resize(ref modes, modes.Length + 1);
            modes[modes.Length - 1] = modes[modes.Length - 2];
            EnforceMode(points.Count - 4);

            if (loop)
            {
                points[points.Count - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                EnforceMode(0);
            }
        }

        public void SetControlPoint(int index, Vector3 point)
        {
            if (index % 3 == 0)
            {
                Vector3 delta = point - points[index];
                if (loop)
                {
                    if (index == 0)
                    {
                        points[1] += delta;
                        points[points.Count - 2] += delta;
                        points[points.Count - 1] = point;
                    }
                    else if (index == points.Count - 1)
                    {
                        points[0] = point;
                        points[1] += delta;
                        points[index - 1] += delta;
                    }
                    else
                    {
                        points[index - 1] += delta;
                        points[index + 1] += delta;
                    }
                }
                else
                {
                    if (index > 0)
                        points[index - 1] += delta;
                    if (index + 1 < points.Count)
                        points[index + 1] += delta;
                }
            }
            points[index] = point;
            EnforceMode(index);
        }

        public BezierControlPointMode GetControlPointMode(int index) => modes[(index + 1) / 3];

        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            int modeIndex = (index + 1) / 3;
            modes[modeIndex] = mode;
            if (loop)
            {
                if (modeIndex == 0)
                    modes[modes.Length - 1] = mode;
                else if (modeIndex == modes.Length - 1)
                    modes[0] = mode;
            }
            EnforceMode(index);
        }

        private void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;
            BezierControlPointMode mode = modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
                return;

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                fixedIndex = fixedIndex < 0 ? points.Count - 2 : fixedIndex;
                enforcedIndex = middleIndex + 1;
                enforcedIndex = enforcedIndex >= points.Count ? 1 : enforcedIndex;
            }
            else
            {
                fixedIndex = middleIndex + 1;
                fixedIndex = fixedIndex >= points.Count ? 1 : fixedIndex;
                enforcedIndex = middleIndex - 1;
                enforcedIndex = enforcedIndex < 0 ? points.Count - 2 : enforcedIndex;
            }

            Vector3 middle = points[middleIndex];
            Vector3 enforcedTangent = middle - points[fixedIndex];
            if (mode == BezierControlPointMode.Aligned)
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);

            points[enforcedIndex] = middle + enforcedTangent;
        }
        
        public void Reset()
        {
            points = new List<Vector3>
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(5f, 0f, 0f),
                new Vector3(10f, 0f, 0f),
                new Vector3(15f, 0f, 0f)
            };

            modes = new BezierControlPointMode[]
            {
                BezierControlPointMode.Free,
                BezierControlPointMode.Free
            };
        }
    }

    public enum BezierControlPointMode
    {
        Free,
        Aligned,
        Mirrored
    }
}
