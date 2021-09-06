using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SimpleCurves
    {
        private static readonly List<Vector3> curves = new List<Vector3>();

        public static List<Vector3> FetchCurves(Vector2 start, Vector2 end, float height, int step = 3)
        {
            if (2 > step) return null;

            curves.Clear();

            float progress, count = step - 1;

            for (int i = 0; i < step; i++)
            {
                progress = i / count;

                Vector3 point = Vector3.Lerp(start, end, progress);

                point.y += Mathf.Lerp(0, height, progress > 0.5f ? 1 - progress : progress) * 2;

                curves.Add(point);
            }

            return curves;
        }
    }
}
