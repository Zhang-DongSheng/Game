using Game;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Test
{
    public class Test2 : MonoBehaviour
    {
        private const int COUNT = 10;

        public GameObject target;

        public float speed = 1f;

        private Material material;

        private readonly List<Pair<Vector3, float>> pairs = new List<Pair<Vector3, float>>();

        private readonly List<float> values = new List<float>();

        private readonly List<Vector4> points = new List<Vector4>();

        private void Awake()
        {
            for (int i = 0; i < COUNT; i++)
            {
                points.Add(Vector4.zero);

                values.Add(0);
            }
            material = target.GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    pairs.Add(new Pair<Vector3, float>()
                    {
                        x = hit.point,
                        y = 1
                    });
                };
            }
            int count = Mathf.Min(pairs.Count, COUNT);

            if (count == 0)
            {
                material.SetInt("_PointCount", 0);
                return;
            }

            for (int i = count - 1; i > -1; i--)
            {
                pairs[i].y -= Time.deltaTime * speed;

                if (pairs[i].y < 0)
                {
                    pairs.RemoveAt(i);
                }
            }
            count = pairs.Count;

            for (int i = 0; i < count; i++)
            {
                points[i] = pairs[i].x;

                values[i] = pairs[i].y;
            }
            material.SetVectorArray("_Points", points);

            material.SetFloatArray("_Values", values);

            material.SetInt("_PointCount", count);
        }
    }
}