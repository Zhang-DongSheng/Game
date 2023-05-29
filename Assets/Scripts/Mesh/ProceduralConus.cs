namespace UnityEngine
{
    /// <summary>
    /// 圆锥
    /// </summary>
    public class ProceduralConus
    {
        private readonly static float P = Mathf.PI;

        private readonly static float P2 = Mathf.PI * 2f;

        public static Mesh CreateConus(int length, int width, float ratio = 1)
        {
            Mesh mesh = new Mesh() { name = string.Format("mesh_{0}X{1}", length, width) };

            int realwidth = width + 1;

            int reallength = length + 1;

            int count = realwidth * reallength + 1;

            #region Vertices and UV
            Vector3[] vertices = new Vector3[count];

            Vector2[] uvs = new Vector2[count];

            vertices[0] = Vector3.up;

            uvs[0] = Vector2.one * 0.5f;

            float w1, w2, l1;

            for (int x = 0; x < realwidth; x++)
            {
                w1 = P * 0.5f * (float)(x + 1) / realwidth;
                w2 = w1 * ratio;

                float sin1 = Mathf.Sin(w1);
                float cos1 = Mathf.Cos(w1);

                for (int z = 0; z < reallength; z++)
                {
                    l1 = P2 * (float)(z == length ? 0 : z) / length;

                    float sin2 = Mathf.Sin(l1);
                    float cos2 = Mathf.Cos(l1);

                    vertices[z + x * reallength + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2);

                    uvs[z + x * reallength + 1] = uvs[0] + new Vector2(cos2, sin2) * (w2 / P);
                }
            }
            #endregion

            #region Normals
            Vector3[] normals = new Vector3[count];
            for (int i = 0; i < count; i++)
                normals[i] = vertices[i].normalized;
            #endregion

            #region Triangles
            int[] triangles = new int[count * 6];

            //Top Cap
            int index = 0;

            for (int z = 0; z < length; z++)
            {
                triangles[index++] = 0;
                triangles[index++] = z + 1;
                triangles[index++] = z + 2;
            }

            //Middle
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    int current = z + x * reallength + 1;
                    int next = current + reallength;

                    triangles[index++] = next + 1;
                    triangles[index++] = current + 1;
                    triangles[index++] = current;

                    triangles[index++] = next;
                    triangles[index++] = next + 1;
                    triangles[index++] = current;
                }
            }
            #endregion

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}