using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BDSZ_2020
{
    public class BDSZ_CanvasDraw : BDSZ_PolygonDraw
    {

        protected CanvasRenderer m_CanvasRender = null;
        public CanvasRenderer CanvasRender { set { m_CanvasRender = value; } get { return m_CanvasRender; } }
 
     
        public override void SetMaterial(Material mat)
        {
            if (m_AttachMaterial != mat)
            {
                m_AttachMaterial = mat;
                if (m_CanvasRender != null)
                {
                    m_CanvasRender.SetMaterial(m_AttachMaterial, null);
                    
                }
            }
        }
        
        public override void SetVisible(bool bVisible)
        {
            if (bVisible == true)
            {
                m_CanvasRender.SetMesh(m_Mesh);
            }
            else
            {
                m_CanvasRender.SetMesh(null);
            }
        }

        public void AddPolygonVertex(Vector3 vPos, Vector2 uv, Color32 clr)
        {
            AddVert(vPos, clr, uv);
        }


        public void SetRenderColor(Color clr)
        {
            if (m_CanvasRender != null)
            {
                m_CanvasRender.SetColor(clr);
            }
        }
        public void CleanVertices()
        {
            if (m_CanvasRender != null)
            {
                if (m_Mesh != null)
                {
                    GameObject.DestroyImmediate(m_Mesh);
                    m_Mesh = null;
                }
                m_CanvasRender.SetMesh(null);// SetVertices(null, 0);
            }
        }
        public override bool EndDraw(PolygonDrawVertexModify modify)
        {

            if (m_Positions == null)
            {
                return false;
            }
            modify?.Invoke(this);
            if (VertexCount == 0)
            {
                m_CanvasRender.SetMesh(null);
            }
            else
            {
                if (m_Mesh == null)
                {
                    m_Mesh = new Mesh();
                    m_Mesh.MarkDynamic();
                }
                //如果mesh 顶点数小于新的顶点数，或者超过太多//
                if (m_Mesh.vertexCount != VertexCount)
                {
                    m_Mesh.Clear();
                }
                int[] triangles = BDSZ_UIParticlesResMgr.Instance.GetQuadrangleIndices(VertexCount / 4);

                m_Mesh.vertices = m_Positions.ToArray();
                m_Mesh.uv = m_Uv0S.ToArray();
                m_Mesh.colors32 = m_Colors.ToArray();
                m_Mesh.SetIndices(triangles, MeshTopology.Triangles, 0);
                m_CanvasRender.SetMesh(m_Mesh);
                
            }
            FreePoolData();
            return true;
        }
        public override void Destroy(bool bDestroy)
        {
            CleanVertices();

            if (bDestroy && m_CanvasRender != null)
            {
                GameObject.DestroyImmediate(m_CanvasRender);
                m_CanvasRender = null;
            }

        }
    }
}