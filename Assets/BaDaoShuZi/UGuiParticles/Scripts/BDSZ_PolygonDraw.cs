using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BDSZ_2020
{
    public class BDSZ_PolygonDraw
    {
        public delegate void PolygonDrawVertexModify(BDSZ_PolygonDraw pd);
        public enum ERectCull
        {
            In,
            Out,
            Intersect,
        }
        protected Mesh m_Mesh = null;
        protected Material m_AttachMaterial = null;
        protected bool m_ListsInitalized = false;
        protected List<Vector3> m_Positions;
        protected List<Color32> m_Colors;
        protected List<Vector2> m_Uv0S;

        protected static bool s_bEnableClipRect = false;
        protected static Rect s_ClipRect = Rect.zero;

        public static void EnableRectClipping(Rect clipRect)
        {
            s_bEnableClipRect = true;
            s_ClipRect = clipRect;
        }
        public static void DisableRectClipping()
        {
            s_bEnableClipRect = false;
        }
        public Material AttachMaterial
        {
            get { return m_AttachMaterial; }
        }

        public virtual void BeginDraw()
        {
            if (m_ListsInitalized == false)
            {
                m_Positions = ListPool<Vector3>.Get();
                m_Colors = ListPool<Color32>.Get();
                m_Uv0S = ListPool<Vector2>.Get();
                m_ListsInitalized = true;
            }
           
        }
        public virtual bool EndDraw(PolygonDrawVertexModify modify)
        {
            return false;
        }
        public virtual void Destroy(bool bDestroy)
        {
            if (m_ListsInitalized == true)
            {
                ListPool<Vector3>.Release(m_Positions);
                ListPool<Color32>.Release(m_Colors);
                ListPool<Vector2>.Release(m_Uv0S);
                m_ListsInitalized = false;
            }
        }
        public virtual void SetVisible(bool bVisible)
        {

        }
        protected virtual void FreePoolData()
        {
            if (m_ListsInitalized)
            {
                m_Positions.Clear();
                m_Colors.Clear();
                m_Uv0S.Clear();
            }
        }
        public int VertexCount { get { return m_Positions.Count; } }
        public bool CanDraw(Material mat)
        {
            if (VertexCount == 0 || m_AttachMaterial == null)
            {
                SetMaterial(mat);
                return true;
            }
            return false;
        }
        public virtual void SetMaterial(Material mat)
        {
            m_AttachMaterial = mat;
        }
        protected void AddVert(Vector3 v,Color32 cr,Vector2 uv)
        {
            m_Positions.Add(v);
            m_Uv0S.Add(uv);
            m_Colors.Add(cr);
        }
        public void DrawQuadrangle(Vector2[] triVecs, Vector2[] triCoords, float fz, Color32 cr, int iVertexCount)
        {
            for (int i = 0; i < iVertexCount; i++)
            {
                AddVert(new Vector3(triVecs[i].x, triVecs[i].y, fz), cr, triCoords[i]);
            }
        }
        public void DrawQuadrangle(Vector2[] triVecs, Vector2[] triCoords, Color32[] cr, float fz, int iVertexCount)
        {
            for (int i = 0; i < iVertexCount; i++)
            {
                AddVert(new Vector3(triVecs[i].x, triVecs[i].y, fz), cr[i], triCoords[i]);
            }
        }
        protected  ERectCull RectCull(Rect src, Rect other)
        {
            if (src.xMin >= other.xMax || src.xMax <= other.xMin)
                return ERectCull.Out;
            if (src.yMin >= other.yMax || src.yMax <= other.yMin)
                return ERectCull.Out;

            if (other.xMin >= src.xMin && other.xMax <= src.xMax && other.yMin >= src.yMin && other.yMax <= src.yMax)
                return ERectCull.In;
            return ERectCull.Intersect;
        }
        public void DrawQuadrangle(Rect rcDraw, Rect rcDrawCoord, float fz, Color32 cr)
        {
            if(s_bEnableClipRect==true)
            {
                ERectCull rc = RectCull( s_ClipRect,rcDraw);
                if (rc == ERectCull.Out)
                    return;
                if(rc==ERectCull.Intersect)
                {
                    Rect rcCullDraw = rcDraw;
                    Rect rcCullUV = rcDrawCoord;
                    ClipQuad(ref rcCullDraw,ref rcDraw, ref rcCullUV,  ref rcDrawCoord, ref s_ClipRect);
                }

            }
            AddVert(new Vector3(rcDraw.xMin, rcDraw.yMin, fz), cr, new Vector2(rcDrawCoord.xMin, rcDrawCoord.yMin));
            AddVert(new Vector3(rcDraw.xMin, rcDraw.yMax, fz), cr, new Vector2(rcDrawCoord.xMin, rcDrawCoord.yMax));
            AddVert(new Vector3(rcDraw.xMax, rcDraw.yMax, fz), cr, new Vector2(rcDrawCoord.xMax, rcDrawCoord.yMax));
            AddVert(new Vector3(rcDraw.xMax, rcDraw.yMin, fz), cr, new Vector2(rcDrawCoord.xMax, rcDrawCoord.yMin));

        }
        public static bool ClipQuad(ref Rect rcQuad, ref Rect rcQuadClip, ref Rect TexCoord, ref Rect TexCoordClip, ref Rect ClipRect)
        {
            if (rcQuad.xMin < ClipRect.xMin)
            {
                TexCoordClip.xMin = TexCoord.xMin + TexCoord.width / rcQuad.width * (ClipRect.xMin - rcQuad.xMin);
                rcQuadClip.xMin = ClipRect.xMin;
            }
            else
            {
                TexCoordClip.xMin = TexCoord.xMin;
                rcQuadClip.xMin = rcQuad.xMin;
            }

            if (rcQuad.xMax > ClipRect.xMax)
            {
                TexCoordClip.xMax = TexCoord.xMax + TexCoord.width / rcQuad.width * (ClipRect.xMax - rcQuad.xMax);
                rcQuadClip.xMax = ClipRect.xMax;
            }
            else
            {
                TexCoordClip.xMax = TexCoord.xMax;
                rcQuadClip.xMax = rcQuad.xMax;
            }


            if (rcQuad.yMin < ClipRect.yMin)
            {
                TexCoordClip.yMin = TexCoord.yMin + TexCoord.height / rcQuad.height * (ClipRect.yMin - rcQuad.yMin);
                rcQuadClip.yMin = ClipRect.yMin;
            }
            else
            {
                TexCoordClip.yMin = TexCoord.yMin;
                rcQuadClip.yMin = rcQuad.yMin;
            }

            if (rcQuad.yMax > ClipRect.yMax)
            {
                TexCoordClip.yMax = TexCoord.yMax + TexCoord.height / rcQuad.height * (ClipRect.yMax - rcQuad.yMax);
                rcQuadClip.yMax = ClipRect.yMax;

            }
            else
            {
                TexCoordClip.yMax = TexCoord.yMax;
                rcQuadClip.yMax = rcQuad.yMax;
            }
            return (rcQuadClip.xMin < rcQuadClip.xMax && rcQuadClip.yMin < rcQuadClip.yMax);
        }
        
        public Vector3 GetPosition(int iIndex)
        {
            return m_Positions[iIndex];
        }
        public void SetPosition(int iIndex,Vector3 p)
        {
            m_Positions[iIndex] = p;
        }
        public void SetVertexColor(int iIndex, Color32 clr)
        {
            m_Colors[iIndex] = clr;
        }

    }
}