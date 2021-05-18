using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class UIPaint : MonoBehaviour
    {
        [Tooltip("图案将被此载体渲染")]
        public Renderer m_rendered;

        [Range(50f, 200f)]
        [Tooltip("线条连贯性(我还没过四级)")]
        public float m_lineSmooth;
        [Tooltip("是否保持图案原有纵横比")]
        public bool m_isKeepRatio;
        [Space(2)]
        [Header("颜色")]
        [Tooltip("绘画时显示的颜色")]
        public Color m_drawColor;
        [Tooltip("在material时的颜色")]
        public Color m_renderedColor;

        private List<List<Vector3>> m_vertexList;
        private void Start()
        {
            m_vertexList = new List<List<Vector3>>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                List<Vector3> newList = new List<Vector3>();
                m_vertexList.Add(newList);
            }
            else if (Input.GetMouseButton(0))
            {
                m_vertexList[m_vertexList.Count - 1].Add(Camera.main.ScreenToViewportPoint(Input.mousePosition));
            }
            else if (Input.GetMouseButtonDown(1))
            {
                PaintGraphics();
            }
        }

        public void OnRenderObject()
        {
            GL.Begin(GL.LINES);
            GL.LoadOrtho();
            GL.Color(m_drawColor);
            int vertexListCount = m_vertexList.Count;

            for (int i = 0; i < vertexListCount; i++)
            {
                int vertexCount = m_vertexList[i].Count;
                for (int j = 1; j < vertexCount; j++)
                {
                    GL.Vertex3(m_vertexList[i][j - 1].x, m_vertexList[i][j - 1].y, 0);
                    GL.Vertex3(m_vertexList[i][j].x, m_vertexList[i][j].y, 0);
                }

            }
            GL.End();
        }
        private void PaintGraphics()
        {
            Texture2D newTexture = new Texture2D(Screen.width, Screen.height);

            //设置像素点
            int vertexListCount = m_vertexList.Count;

            for (int i = 0; i < vertexListCount; i++)
            {
                int vertexCount = m_vertexList[i].Count;
                for (int j = 1; j < vertexCount; j++)
                {
                    float x = m_vertexList[i][j - 1].x;
                    float y = m_vertexList[i][j - 1].y;
                    for (int k = 0; k <= (int)m_lineSmooth; k++)
                    {
                        x = Mathf.Lerp(x, m_vertexList[i][j].x, 1 / m_lineSmooth);
                        y = Mathf.Lerp(y, m_vertexList[i][j].y, 1 / m_lineSmooth);
                        newTexture.SetPixel((int)(x * newTexture.width), (int)(y * newTexture.height), m_renderedColor);
                        //Debug.Log((int)(x * newTexture.width) + " " + (int)(y * newTexture.height));
                    }
                }

            }
            newTexture.Apply();
            //赋值
            m_rendered.material.mainTexture = newTexture;
            if (m_isKeepRatio)
                this.transform.localScale = new Vector3(1f, Screen.height * 1.0f / Screen.width, 1f);
            m_vertexList.Clear();

        }
    }
}