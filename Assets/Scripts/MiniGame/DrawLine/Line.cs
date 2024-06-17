using System.Collections.Generic;
using UnityEngine;

namespace Game.MiniGame
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;

        [SerializeField] private Rigidbody2D rigidbody2d;

        [SerializeField] private EdgeCollider2D edgeCollider;

        private float distance;

        private int count;

        private readonly List<Vector2> points = new List<Vector2>();

        public void Instantiate(float width, Gradient color, float distance, LayerMask layer)
        {
            var radius = width / 2f;

            this.distance = distance;

            lineRenderer.startWidth = width;

            lineRenderer.endWidth = width;

            lineRenderer.colorGradient = color;

            edgeCollider.edgeRadius = radius;

            rigidbody2d.gravityScale = 0;

            rigidbody2d.isKinematic = true;

            gameObject.layer = 1 << layer;
        }


        public void AddPoint(Vector2 point)
        {
            if (count > 0 && Vector2.Distance(point, points[count - 1]) < distance)
                return;
            count = points.Count;

            lineRenderer.positionCount = count + 1;

            lineRenderer.SetPosition(count, point);

            points.Add(point);

            edgeCollider.points = points.ToArray();
        }

        public void Over()
        {
            if (points.Count > 2)
            {
                gameObject.layer = 1 << LayerMask.NameToLayer("Default");

                rigidbody2d.gravityScale = 9.8f;

                rigidbody2d.isKinematic = false;
            }
            else
            { 
                GameObject.Destroy(gameObject);
            }
        }
    }
}