using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Test
{
    public class Test2 : MonoBehaviour
    {
        public GameObject target;

        public float speed = 1f;

        private Material material;

        private readonly List<Vector4> points = new List<Vector4>();

        private readonly List<float> values = new List<float>();

        private void Awake()
        {
            material = target.GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    points.Add(hit.point);

                    values.Add(1);
                };
            }
            int count = points.Count;

            if (count == 0)
            {
                material.SetInt("_PointCount", 0);
                return;
            }

            for (int i = count - 1; i > -1; i--)
            {
                values[i] -= Time.deltaTime * speed;

                if (values[i] < 0)
                {
                    values.RemoveAt(i);

                    points.RemoveAt(i);
                }
            }
            material.SetVectorArray("_Points", points);

            material.SetFloatArray("_Values", values);

            material.SetInt("_PointCount", count);

            Debug.Log($"当前有{count}点");

            Debug.Log(string.Join(',', points));
        }
    }
}
