using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraCullingGroup : MonoBehaviour
    {
        [SerializeField] private float distance = 100f;

        protected CullingGroup culling;

        protected readonly List<Renderer> renderers = new List<Renderer>();

        protected readonly List<BoundingSphere> bounds = new List<BoundingSphere>();

        private void Start()
        {
            culling = new CullingGroup();

            Camera camera = GetComponent<Camera>();

            culling.targetCamera = camera;

            culling.SetDistanceReferencePoint(transform);

            culling.SetBoundingDistances(new float[] { distance, float.PositiveInfinity });

            Relocation();

            culling.onStateChanged += OnStateChanged;
        }

        protected virtual void OnStateChanged(CullingGroupEvent sphere)
        {
            if (sphere.isVisible)
            {
                if (sphere.currentDistance == 0)
                {
                    renderers[sphere.index].material.color = Color.green;
                }
                else
                {
                    renderers[sphere.index].material.color = Color.red;
                }
            }
            else
            {
                renderers[sphere.index].material.color = Color.gray;
            }
        }

        protected virtual void Relocation()
        {
            renderers.Clear(); bounds.Clear();

            renderers.AddRange(FindObjectsOfType<Renderer>());

            int count = renderers.Count;

            for (int i = 0; i < count; i++)
            {
                if (renderers[i].TryGetComponent(out MeshFilter filter) && filter.mesh != null)
                {
                    bounds.Add(new BoundingSphere(renderers[i].transform.position,
                        filter.mesh.bounds.extents.x));
                }
                else
                {
                    bounds.Add(new BoundingSphere(renderers[i].transform.position,
                        renderers[i].transform.localScale.x));
                }
            }
            culling.SetBoundingSpheres(bounds.ToArray());

            culling.SetBoundingSphereCount(count);
        }

        private void OnDestroy()
        {
            culling.onStateChanged -= OnStateChanged;

            culling.Dispose();
        }
    }
}