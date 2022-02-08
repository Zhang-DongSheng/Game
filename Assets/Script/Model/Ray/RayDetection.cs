using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    public static class RayDetection
    {
        public static bool DetectionPass(List<Vector3> vectors, LayerMask layer)
        {
            bool pass = true;

            int count = vectors.Count;

            for (int i = 1; i < count; i++)
            {
                if (Physics.Linecast(vectors[i - 1], vectors[i], layer))
                {
                    pass = false;
                    break;
                }
            }
            return pass;
        }

        public static bool DetectionPass(Vector3 start, Vector3 end, LayerMask layer)
        {
            bool pass = true;

            if (Physics.Linecast(start, end, layer))
            {
                pass = false;
            }
            return pass;
        }

        public static bool DetectionPass(Vector3 start, Vector3 end)
        {
            bool pass = true;

            if (Physics.Linecast(start, end))
            {
                pass = false;
            }
            return pass;
        }

        public static bool DetectionInside(Vector3 point, Vector3 direction, Collider collider)
        {
            Ray ray = new Ray(point, direction);

            if (collider.bounds.IntersectRay(ray))
            {
                return true;
            }
            return false;
        }
    }
}