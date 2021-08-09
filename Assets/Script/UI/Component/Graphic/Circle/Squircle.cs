using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UI
{
    public class Squircle : MaskableGraphic
    {
        const float C = 1.0f;
        [Space]
        [SerializeField] private Type squircleType = Type.Scaled;
        [Range(1, 40)]
        [SerializeField] private float n = 4;
        [Min(0.1f)]
        [SerializeField] private float delta = 5f;
        [SerializeField] private float quality = 0.1f;
        [Min(0)]
        [SerializeField] private float radius = 1000;

        private float a, b;

        private readonly List<Vector2> vert = new List<Vector2>();

        private float SquircleFunc(float t, bool xByY)
        {
            if (xByY)
                return (float)System.Math.Pow(C - System.Math.Pow(t / a, n), 1f / n) * b;

            return (float)System.Math.Pow(C - System.Math.Pow(t / b, n), 1f / n) * a;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            float dx = 0;

            float dy = 0;

            float width = rectTransform.rect.width / 2;

            float height = rectTransform.rect.height / 2;

            if (squircleType == Type.Classic)
            {
                a = width;
                b = height;
            }
            else
            {
                a = Mathf.Min(width, height, radius);
                b = a;

                dx = width - a;
                dy = height - a;
            }

            float x = 0, y = 1;

            vert.Clear();

            vert.Add(new Vector2(0, height));

            while (x < y)
            {
                y = SquircleFunc(x, true);
                vert.Add(new Vector2(dx + x, dy + y));
                x += delta;
            }

            if (float.IsNaN(vert.Last().y))
            {
                vert.RemoveAt(vert.Count - 1);
            }

            while (y > 0)
            {
                x = SquircleFunc(y, false);
                vert.Add(new Vector2(dx + x, dy + y));
                y -= delta;
            }

            vert.Add(new Vector2(width, 0));

            for (int i = 1; i < vert.Count - 1; i++)
            {
                if (vert[i].x < vert[i].y)
                {
                    if (vert[i - 1].y - vert[i].y < quality)
                    {
                        vert.RemoveAt(i);
                        i -= 1;
                    }
                }
                else
                {
                    if (vert[i].x - vert[i - 1].x < quality)
                    {
                        vert.RemoveAt(i);
                        i -= 1;
                    }
                }
            }

            vert.AddRange(vert.AsEnumerable().Reverse().Select(t => new Vector2(t.x, -t.y)));
            vert.AddRange(vert.AsEnumerable().Reverse().Select(t => new Vector2(-t.x, t.y)));

            vh.Clear();

            for (int i = 0; i < vert.Count - 1; i++)
            {
                vh.AddVert(vert[i], color, Vector2.zero);

                vh.AddVert(vert[i + 1], color, Vector2.zero);

                vh.AddVert(Vector2.zero, color, Vector2.zero);

                vh.AddTriangle(i * 3, i * 3 + 1, i * 3 + 2);
            }
        }

        enum Type
        {
            Classic,
            Scaled
        }
    }
}