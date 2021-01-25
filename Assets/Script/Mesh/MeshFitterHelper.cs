using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshFitterHelper : MonoBehaviour
    {
        [SerializeField] private MeshFilter filter;

        [SerializeField] private MeshEnum type;

        [SerializeField, Range(-10, 10)] private float height;

        private void Awake()
        {
            if (filter == null)
                filter = GetComponent<MeshFilter>();
        }

        private void Start()
        {
            //filter.mesh = ProceduralHemispherePolarUVs.hemisphere;
        }

        private void OnValidate()
        {
            switch (type)
            {
                case MeshEnum.Sphere1:
                    filter.mesh = ProceduralHemisphere.hemisphere;
                    break;
                case MeshEnum.Sphere2:
                    filter.mesh = ProceduralHemisphere.hemisphereInv;
                    break;
                case MeshEnum.Conus:
                    filter.mesh = ProceduralConus.CreateConus(48, 32, 1);
                    break;
                case MeshEnum.TriangularPrism:
                    filter.mesh = ProceduralTriangularPrism.Create(height);
                    break;
            }
        }

        enum MeshEnum
        {
            Sphere1,
            Sphere2,
            Conus,
            TriangularPrism,
        }
    }
}