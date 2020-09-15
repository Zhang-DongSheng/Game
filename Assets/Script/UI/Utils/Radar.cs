using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Radar : MonoBehaviour
{
    [SerializeField] private float radius = 45;

    [SerializeField] private Color origin = Color.white;

    [SerializeField] private Color destination = Color.red;

    [SerializeField, Range(0, 1)] private List<float> attributes;

    private float m_cell_angle;

    private int[] m_triangles;

    private Color[] m_colors;

    private List<Vector3> m_vertices = new List<Vector3>();

    private MeshRenderer m_render;

    private MeshFilter m_filter;

    private Mesh m_mesh;

    private void Awake()
    {
        m_render = GetComponent<MeshRenderer>();

        m_filter = GetComponent<MeshFilter>();

        m_mesh = new Mesh();

        m_filter.mesh = m_mesh;

        if (m_render.material == null)
        {
            Debug.LogWarning("请添加Material");
        }
    }

    private void Start()
    {
        if (attributes.Count != 0)
        {
            m_cell_angle = 360 / attributes.Count;

            InitMeshData(); Refresh();
        }
    }

    private void OnValidate()
    {
        Refresh();
    }

    private void InitMeshData()
    {
        #region 设置顶点
        m_vertices.Clear();
        m_vertices.Add(new Vector3(0, 0, 1));
        m_vertices.Add(new Vector3(radius, 0, 1));

        for (int i = 0; i < attributes.Count; i++)
        {
            float angle = Mathf.Deg2Rad * m_cell_angle * (i + 1);
            Vector3 point = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 1);
            m_vertices.Add(point);
        }
        #endregion

        #region 设置三角形
        m_triangles = new int[attributes.Count * 3];

        int index = 0;
        int value = 0;
        for (int i = 0; i < m_triangles.Length; i++)
        {
            if (i % 3 == 0)
            {
                m_triangles[i] = 0;
                value = index;
                index++;
            }
            else
            {
                value++;
                if (value == attributes.Count + 1)
                    value = 1;
                m_triangles[i] = value;
            }
        }
        #endregion

        #region Color
        m_colors = new Color[attributes.Count + 2];

        for (int i = 0; i < m_colors.Length; i++)
        {
            m_colors[i] = origin;
        }
        #endregion
    }

    private void Refresh()
    {
        if (m_mesh == null) return;

        Vector3[] vertices = m_vertices.ToArray();

        for (int i = 1; i < m_vertices.Count - 1; i++)
        {
            m_colors[i] = Color.Lerp(origin, destination, attributes[i - 1]);
            vertices[i] = m_vertices[i] * attributes[i - 1];
        }

        m_mesh.vertices = vertices;
        m_mesh.colors = m_colors;
        m_mesh.triangles = m_triangles;
        m_mesh.RecalculateNormals();
    }

    public void Refresh(int index, float value)
    {
        if (attributes.Count > index)
        {
            attributes[index] = value;
        }
        Refresh();
    }
}