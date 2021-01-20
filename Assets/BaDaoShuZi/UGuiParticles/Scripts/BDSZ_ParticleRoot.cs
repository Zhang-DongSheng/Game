using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace BDSZ_2020
{
    [ExecuteInEditMode]
    public class BDSZ_ParticleRoot : MaskableGraphic
    {

        [SerializeField]
        protected bool m_bClip = false;

        private List<BDSZ_CanvasDraw> m_SpriteRenders = new List<BDSZ_CanvasDraw>();
        private List<BDSZ_UIEffectController> m_Effects = new List<BDSZ_UIEffectController>();
        private Rect m_ClipRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

        protected BDSZ_ParticleRoot()
        {
        }

        public void SetVisible(bool bVisible)
        {
            for (int i = 0; i < m_SpriteRenders.Count; i++)
            {
                m_SpriteRenders[i].SetVisible(bVisible);
            }
        }
        protected  void UnloadResource()
        {
            for (int i = 0; i < m_SpriteRenders.Count; i++)
            {
                m_SpriteRenders[i].Destroy(false);
                m_SpriteRenders[i] = null;
            }
            m_SpriteRenders.Clear();
        }
        protected void Update()
        {
            int iRedraw = 0;
            Rect rc = GetPixelAdjustedRect();
            if (IsActive())
            {
                for (int i = 0; i < m_Effects.Count; i++)
                {
                    if (m_Effects[i].UpdateEffect(rc))
                        iRedraw++;
                }
            }
          
            if (iRedraw > 0)
            {
                SetVerticesDirty();
            }
#if UNITY_EDITOR
            UpdateGeometry();
#endif
        }
        protected override void UpdateGeometry()
        {
            Vector3[] wc = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(wc);        // 计算world space中的点坐标
            m_ClipRect =  Rect.MinMaxRect(wc[0].x/Screen.width*2.0f-1.0f, wc[0].y/Screen.height*2.0f-1.0f, wc[2].x/Screen.width*2.0f-1.0f, wc[2].y/Screen.height*2.0f-1.0f);// 选取左下角和右上角
             for (int i = 0; i < m_SpriteRenders.Count; i++)
            {
                m_SpriteRenders[i].BeginDraw();
            }

            for (int i = 0; i < m_Effects.Count; i++)
            {
                m_Effects[i].Draw(this);
            }
 
            for (int i = 0; i < m_SpriteRenders.Count; i++)
            {
                m_SpriteRenders[i].EndDraw(null);
            }
            
        }

        public BDSZ_CanvasDraw FindCanvasRender(Material mat, int iDepthOffset)
        {
            BDSZ_CanvasDraw rd = null;
                           
          
            for (int i = 0; i < m_SpriteRenders.Count; i++)
            {
                if (m_SpriteRenders[i].AttachMaterial == mat || (iDepthOffset == 0 && m_SpriteRenders[i].CanDraw(mat)))
                {
                    rd = m_SpriteRenders[i];
                    //rd.BeginDraw();
                    break;
                }
            }
            if (rd == null)
            {
                int iIndex = m_SpriteRenders.Count;
                rd = RequestRenderer(mat, iIndex + iDepthOffset);
                m_SpriteRenders.Add(rd);
            }
            
            if (m_bClip == true)
            {
                rd.CanvasRender.EnableRectClipping(m_ClipRect);
            }
            else
            {
                rd.CanvasRender.DisableRectClipping();
            }
            return rd;
        }
        
 
        public void AddEffect(BDSZ_UIEffectController effect)
        {
            if (m_Effects.Contains(effect) == false)
            {
                m_Effects.Add(effect);
            }
        }
  
        /// <summary>
        /// Returns the texture used to draw this Graphic.
        /// </summary>
        public override Texture mainTexture
        {
            get
            {

                if (material != null && material.mainTexture != null)
                {
                    return material.mainTexture;
                }
                return s_WhiteTexture;

            }
        }
        public BDSZ_CanvasDraw RequestRenderer(Material mat, int iRelativeDepth)
        {
            GameObject find = gameObject;
            string strObjectName = BDSZ_UIParticlesUtilities.c_strCanvasRender + iRelativeDepth.ToString();
            if (iRelativeDepth > 0)
            {
                Transform childNode = find.transform.Find(strObjectName);
                if (childNode == null)
                {
                    int iBestIndex = BDSZ_UIParticlesUtilities.FindBestRenderPosition(gameObject.transform);
                    int iMaxDepth = 0;
                    for (int ic = 0; ic < gameObject.transform.childCount; ic++)
                    {
                        int iDepth = BDSZ_UIParticlesUtilities.GetRelativeDepthFromName(gameObject.transform.GetChild(ic));
                        iMaxDepth = Mathf.Max(iMaxDepth, iDepth);
                        if (iRelativeDepth < iDepth)
                        {
                            iBestIndex = ic;
                            break;
                        }
                    }
                    if (iMaxDepth == 0)
                        iBestIndex = 0;
#if _DEBUG_CREATE_CHILD_
                    find = BDSZ_FontUtilities.CreateChildGo(gameObject, strObjectName, HideFlags.DontSave);
#else
                    find = BDSZ_UIParticlesUtilities.CreateChildGo(gameObject, strObjectName, HideFlags.HideAndDontSave);
#endif


                    if (iBestIndex != gameObject.transform.childCount - 1)
                    {
                        find.transform.SetSiblingIndex(iBestIndex);
                    }
                }
                else
                {
#if _DEBUG_CREATE_CHILD_
                    childNode.hideFlags = HideFlags.DontSave;
                    if (find != gameObject)
                    {
                        find.hideFlags = HideFlags.DontSave;
                    }
#endif
                    find = childNode.gameObject;
                }

            }
            CanvasRenderer cr = find.GetComponent<CanvasRenderer>();

            if (cr == null)
            {
                cr = find.AddComponent<CanvasRenderer>();
                cr.hideFlags = HideFlags.HideAndDontSave;
            }
            if (cr != null)
            {
#if _DEBUG_CREATE_CHILD_
                cr.hideFlags = HideFlags.DontSave;
#endif
                cr.SetMaterial(mat, null);
            }
            if (iRelativeDepth == 0)
            {
                m_Material = mat;
            }

            BDSZ_CanvasDraw uiRender = new BDSZ_CanvasDraw();
            uiRender.SetMaterial(mat);
            uiRender.CanvasRender = cr;
            uiRender.BeginDraw();
            return uiRender;
        }
 

    }
}
