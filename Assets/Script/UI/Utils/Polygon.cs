using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// 自定义多边形
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class Polygon : BaseMeshEffect
    {
        [SerializeField] private List<Vector2> points;

        [SerializeField] private Color32 color;

        public override void ModifyMesh(VertexHelper helper)
        {
            helper.Clear();

            //设置坐标点
            foreach (var point in points)
            {
                helper.AddVert(point, color, new Vector2(0f, 0f));
            }

            int count = points.Count;

            //自定义三角形
            for (int i = 0; i < count - 1; i++)
            {
                helper.AddTriangle(i, Index(i + 1, count), Index(i + 2, count));
            }
        }

        private int Index(int index, int max)
        {
            if (index < max)
            {
                while (index < 0 && max > 0)
                {
                    index += max;
                }
            }
            else
            {
                index %= max;
            }
            return index;
        }
    }
}