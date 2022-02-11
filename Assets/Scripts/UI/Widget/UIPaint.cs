using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class UIPaint : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        [SerializeField, Range(1, 30)] private int density = 3;

        [SerializeField, Range(1, 5)] private int ratio = 1;

        [SerializeField, Range(0, 5f)] private float thickness = 1;

        [SerializeField] private Color color;

        private Vector2 start, end, node = new Vector2(0, 0);

        private int width, height;

        private Texture2D texture;

        private readonly List<Vector2> line = new List<Vector2>();

        private readonly List<Vector2> nodes = new List<Vector2>();

        private readonly List<Vector2> offset = new List<Vector2>(9)
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(-1, 1),
            new Vector2(-1, 0),
            new Vector2(-1, -1),
            new Vector2(0, -1),
            new Vector2(1, -1)
        };

        private void Awake()
        {
            width = Screen.width;

            height = Screen.height;

            texture = new Texture2D(width, height);

            sprite.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                line.Clear(); line.Add(Camera.main.ScreenToViewportPoint(Input.mousePosition));
            }
            else if (Input.GetMouseButton(0))
            {
                line.Add(Camera.main.ScreenToViewportPoint(Input.mousePosition));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                PaintGraphics();
            }
        }

        private void OnRenderObject()
        {
            GL.Begin(GL.LINES);
            GL.LoadOrtho();
            GL.Color(color);
            Compute();
            for (int i = 0; i < nodes.Count; i++)
            {
                GL.Vertex3(nodes[i].x, nodes[i].y, 0);
            }
            GL.End();
        }

        private void PaintGraphics()
        {
            Vector2 point; Compute();

            for (int i = 0; i < nodes.Count; i++)
            {
                node.x = nodes[i].x * width;

                node.y = nodes[i].y * height;

                texture.SetPixel((int)node.x, (int)node.y, color);

                if (thickness > 0)
                {
                    for (int k = 1; k < offset.Count; k++)
                    {
                        for (int v = 1; v <= ratio; v++)
                        {
                            point = node + Vector2.Lerp(Vector2.zero, offset[k], (float)v / ratio) * thickness;

                            texture.SetPixel((int)point.x, (int)point.y, color);
                        }
                    }
                }
            }
            texture.Apply(); line.Clear();
        }

        private void Compute()
        {
            nodes.Clear();

            for (int i = 1; i < line.Count; i++)
            {
                start = line[i - 1]; end = line[i];

                for (int j = 0; j < density; j++)
                {
                    nodes.Add(Vector2.Lerp(start, end, j / (float)density));
                }
            }
        }
    }
}