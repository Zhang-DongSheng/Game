namespace UnityEngine
{
    /// <summary>
    /// 半球
    /// </summary>
    public class ProceduralHemisphere
    {
        private static Mesh _hemisphere;
        public static Mesh hemisphere
        {
            get
            {
                if (_hemisphere == null)
                {
                    CreateProceduralHemisphereWithUVs();
                }
                return _hemisphere;
            }
        }

        private static Mesh _hemisphereInv;
        public static Mesh hemisphereInv
        {
            get
            {
                if (_hemisphereInv == null)
                {
                    CreateProceduralHemisphereWithUVs();
                }
                return _hemisphereInv;
            }
        }

        // modified http://wiki.unity3d.com/index.php/ProceduralPrimitives
        private static void CreateProceduralHemisphereWithUVs()
        {
            _hemisphere = new Mesh() { name = "procedurally_created_hemisphere" };
            _hemisphere.Clear();

            _hemisphereInv = new Mesh() { name = "procedurally_created_inverted_hemisphere" };
            _hemisphereInv.Clear();

            // Longitude |||
            int nbLong = 6; //48 //24
                            // Latitude ---
            int nbLat = 4; //32 //16

            #region Vertices and UVsss
            Vector3[] vertices = new Vector3[(nbLong + 1) * (nbLat + 1) + 1];
            Vector2[] uvs = new Vector2[vertices.Length];

            float _pi = Mathf.PI;
            float _2pi = _pi * 2f;

            vertices[0] = Vector3.up;
            uvs[0] = new Vector2(0.5f, 0.5f);

            for (int lat = 0; lat < nbLat + 1; lat++)
            {
                float a11 = _pi / 1.98f * (float)(lat + 1) / (nbLat + 1);
                float a12 = _pi / 2.0f * (float)(lat + 1) / (nbLat + 1);
                float sin1 = Mathf.Sin(a11);
                float cos1 = Mathf.Cos(a11);

                for (int lon = 0; lon <= nbLong; lon++)
                {
                    float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                    float sin2 = Mathf.Sin(a2);
                    float cos2 = Mathf.Cos(a2);

                    vertices[lon + lat * (nbLong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2);

                    uvs[lon + lat * (nbLong + 1) + 1] = uvs[0] + new Vector2(cos2, sin2) * (a12 / _pi);
                    // A vertex position can easily be reverse calculated from these UVs
                }
            }
            #endregion

            #region Normals	ss	
            Vector3[] normals = new Vector3[vertices.Length];
            for (int n = 0; n < vertices.Length; n++)
                normals[n] = -vertices[n].normalized;

            Vector3[] normals2 = new Vector3[vertices.Length];
            for (int n = 0; n < vertices.Length; n++)
                normals2[n] = vertices[n].normalized;
            #endregion

            #region Triangles
            int nbFaces = vertices.Length;
            int nbTriangles = nbFaces * 2;
            int nbIndexes = nbTriangles * 3;
            int[] triangles = new int[nbIndexes];
            int[] triangles2 = new int[nbIndexes];

            //Top Cap
            int i = 0; int j = 0;
            for (int lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = 0;
                triangles[i++] = lon + 1;
                triangles[i++] = lon + 2;

                triangles2[j++] = lon + 2;
                triangles2[j++] = lon + 1;
                triangles2[j++] = 0;
            }

            //Middle
            for (int lat = 0; lat < nbLat; lat++)
            {
                for (int lon = 0; lon < nbLong; lon++)
                {
                    int current = lon + lat * (nbLong + 1) + 1;
                    int next = current + nbLong + 1;

                    triangles[i++] = next + 1;
                    triangles[i++] = current + 1;
                    triangles[i++] = current;

                    triangles[i++] = next;
                    triangles[i++] = next + 1;
                    triangles[i++] = current;

                    triangles2[j++] = current;
                    triangles2[j++] = current + 1;
                    triangles2[j++] = next + 1;

                    triangles2[j++] = current;
                    triangles2[j++] = next + 1;
                    triangles2[j++] = next;
                }
            }

            #endregion

            _hemisphere.vertices = vertices;
            _hemisphere.normals = normals;
            _hemisphere.uv = uvs;
            _hemisphere.triangles = triangles;

            _hemisphere.RecalculateBounds();

            _hemisphereInv.vertices = vertices;
            _hemisphereInv.normals = normals2;
            _hemisphereInv.uv = uvs;
            _hemisphereInv.triangles = triangles2;

            _hemisphereInv.RecalculateBounds();
        }
    }
}