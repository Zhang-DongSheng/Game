using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicHelper : BaseMeshEffect
{
    [SerializeField] private List<Vector3> points;

    public override void ModifyMesh(VertexHelper helper)
    {
        helper.Clear();

        //设置坐标点
        foreach (var point in points)
        {
            helper.AddVert(point, Random.ColorHSV(), new Vector2(0f, 0f));
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