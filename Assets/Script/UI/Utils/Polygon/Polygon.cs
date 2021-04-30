using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// 自定义多边形
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class Polygon : MaskableGraphic
    {
        [SerializeField]
        private List<Vector2> points = new List<Vector2>()
        {
            new Vector2(50, 50), new Vector2(50, -50), new Vector2(-50, -50), new Vector2(-50, 50)
        };

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear();

            if (points != null && points.Count > 0)
            {
                int count = points.Count;

                //设置坐标点
                foreach (var point in points)
                {
                    helper.AddVert(point, color, Vector2.zero);
                }

                //自定义三角形
                for (int i = 1; i < count - 1; i++)
                {
                    helper.AddTriangle(i, 0, i + 1);
                }
            }
        }
    }
} 