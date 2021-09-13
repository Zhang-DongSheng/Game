using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    public static class RayDetection
    {
        public static bool Detection(List<Vector3> vectors, LayerMask layer)
        {
            bool pass = true;

            int count = vectors.Count;

            for (int i = 1; i < count; i++)
            {
                if (Physics.Raycast(vectors[i - 1], vectors[i] - vectors[i - 1], Vector3.Distance(vectors[i], vectors[i - 1]), layer))
                {
                    pass = false;
                    break;
                }
            }
            return pass;
        }
    }
}