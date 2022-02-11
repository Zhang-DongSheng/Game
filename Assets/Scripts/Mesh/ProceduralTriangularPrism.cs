using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 三棱体
/// </summary>
public class ProceduralTriangularPrism
{




    public static Mesh Create(float height = 1)
    {
        Mesh mesh = new Mesh() { name = string.Format("mesh_{0}", height) };

        float no3 = Mathf.Sqrt(3);

        int count = 4;

        #region Vertices and UV
        Vector3[] vertices = new Vector3[count];

        Vector2[] uvs = new Vector2[count];

        vertices[0] = Vector3.up * height;

        vertices[1] = new Vector3(0, 0, 2);

        vertices[2] = new Vector3(no3 * -1, 0, -1);

        vertices[3] = new Vector3(no3, 0, -1);

        uvs[3] = Vector2.one;

        uvs[2] = Vector2.one * 0.66f;

        uvs[1] = Vector2.one * 0.33f;

        uvs[0] = Vector2.zero;
        #endregion

        #region Normals
        Vector3[] normals = new Vector3[count];
        for (int i = 0; i < count; i++)
            normals[i] = vertices[i].normalized;
        #endregion

        #region Triangles
        int[] triangles = new int[3 * count * 2];

        int index = 0;

        triangles[index++] = 0;
        triangles[index++] = 1;
        triangles[index++] = 2;

        triangles[index++] = 0;
        triangles[index++] = 1;
        triangles[index++] = 3;

        triangles[index++] = 0;
        triangles[index++] = 2;
        triangles[index++] = 3;

        triangles[index++] = 1;
        triangles[index++] = 2;
        triangles[index++] = 3;

        triangles[index++] = 2;
        triangles[index++] = 1;
        triangles[index++] = 0;

        triangles[index++] = 3;
        triangles[index++] = 1;
        triangles[index++] = 0;

        triangles[index++] = 3;
        triangles[index++] = 2;
        triangles[index++] = 0;

        triangles[index++] = 3;
        triangles[index++] = 2;
        triangles[index++] = 1;
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();

        return mesh;
    }
}
