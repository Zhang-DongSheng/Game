 
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Serialization;

namespace BDSZ_2020
{
 
    [Serializable]
 
    public class BDSZ_EffectTextureRes 
    {

        //需要保存的数据 ================================================
        [SerializeField]
        protected Texture m_Texture;
        [SerializeField]
        protected bool m_bGrid;
        [SerializeField]
        protected int m_iTileX;
        [SerializeField]
        protected int m_iTileY;
        [SerializeField]
        protected int m_iTileIndex;
        [SerializeField]
        [Range(0.0f,1.0f)]
        protected float m_fInterval=0.0f;

        [SerializeField]
        protected float m_fXVelocity=0.0f;
        [SerializeField]
        protected float m_fYVelocity=0.0f;
        [SerializeField]
        protected float m_fRollVelocity=0.0f;

        [SerializeField]
        protected ETextureFlip m_Flip = ETextureFlip.Nothing;
        //需要保存的数据 ================================================
       
        protected Rect m_rcTextureCoord = new Rect(0.0f,0.0f,1.0f,1.0f);
        protected Vector2[] m_UVs = new Vector2[4];
        public BDSZ_EffectTextureRes()
        {
            //
        }
        public static BDSZ_EffectTextureRes defaultRes
        {
            get
            {
                var spriteData = new BDSZ_EffectTextureRes
                {
                     
                    m_bGrid = false,
                    m_iTileX =1,
                    m_iTileY=1,
                    m_iTileIndex = 0,
                    m_fInterval=0.0f,
                    m_fXVelocity =0.0f,
                    m_fYVelocity = 0.0f,
                    m_fRollVelocity=0.0f,
                  
                };
                return spriteData;
            }
        }
       
        
        public bool IsCellAnimation()
        {
            if (m_bGrid==false)
                return false;
            return m_fInterval > 0.0f;
        }
    
        public int GetCellIndex()
        {
            if (IsCellAnimation())
                return 0;
            if (m_iTileIndex<0)
                return UnityEngine.Random.Range(0, m_iTileX * m_iTileY);
            return m_iTileIndex;
        }
        public int UpdateCellIndex(float fTime,int iOldIndex)
        {
            UpdateUVMove(fTime);
            if (m_fInterval == 0.0f)
                return iOldIndex;
            return Mathf.FloorToInt(fTime / m_fInterval) % (m_iTileX * m_iTileY);

        }
        public bool Grid { get { return m_bGrid; } set { m_bGrid = value; } }

        public Rect GetUVRect(int iCellIndex)
        {
            Rect rc = m_rcTextureCoord;
            int ix = iCellIndex % m_iTileX;
            int iy = iCellIndex / m_iTileX;

            rc.xMin = (float)ix / (float)m_iTileX;
            rc.yMin = (float)iy / (float)m_iTileY;
            rc.xMax = (float)(ix + 1.0f) / (float)m_iTileX;
            rc.yMax = (float)(iy + 1.0f) / (float)m_iTileY;
            return rc;
        }
        public Vector2[] GetUVs(int iCellIndex)
        {
            if (m_fInterval > 0.0f || m_bGrid == true)
            {
                int ix = iCellIndex % m_iTileX;
                int iy = iCellIndex / m_iTileX;

                Vector2 uvLeft;
                uvLeft.x = ix * m_rcTextureCoord.width;
                uvLeft.y = iy * m_rcTextureCoord.height;
                m_UVs[0] = new Vector2(uvLeft.x, uvLeft.y + m_rcTextureCoord.height);
                m_UVs[1] = new Vector2(uvLeft.x + m_rcTextureCoord.width, uvLeft.y + m_rcTextureCoord.height);
                m_UVs[2] = new Vector2(uvLeft.x + m_rcTextureCoord.width, uvLeft.y);
                m_UVs[3] = new Vector2(uvLeft.x, uvLeft.y);
                return m_UVs;
            }
            else
            {
                return m_UVs;
            }
        }
        void UpdateUVMove(float fDeltaTime)
        {
            if (m_bGrid == true)
                return;
            if (m_fXVelocity != 0.0f || m_fYVelocity!=0.0f)
            {
                float fTemp = m_fXVelocity * fDeltaTime;
                float fx = fTemp - Mathf.FloorToInt(fTemp);
                fTemp = m_fYVelocity * fDeltaTime;
                float fy = fTemp - Mathf.FloorToInt(fTemp);

                m_UVs[0] = new Vector2(m_rcTextureCoord.xMin+fx, m_rcTextureCoord.yMax+fy);
                m_UVs[1] = new Vector2(m_rcTextureCoord.xMax+fx, m_rcTextureCoord.yMax+fy);
                m_UVs[2] = new Vector2(m_rcTextureCoord.xMax+fx, m_rcTextureCoord.yMin+fy);
                m_UVs[3] = new Vector2(m_rcTextureCoord.xMin+fx, m_rcTextureCoord.yMin+fy);
            }
        }
        public void ApplayMaterialTextureMat(Material mat)
        {
            //mat.mainTextureOffset
            Vector2 vScale = mat.mainTextureScale;
            Vector2 vOffset = mat.mainTextureOffset;
            if ((m_Flip & ETextureFlip.Vertically) != 0)
            {
                vScale.y *= -1.0f;
                vOffset.y = 1.0f;
            }
            if ((m_Flip & ETextureFlip.Horizontally) != 0)
            {
                vScale.x *= -1.0f;
                vOffset.x = 1.0f;
            }
            mat.mainTextureScale = vScale;
            mat.mainTextureOffset = vOffset;
        }
 
        public Texture MainTexture
        {
            get { return m_Texture == null ? Texture2D.whiteTexture : m_Texture; }
            set
            {
                AttachTextureRes(value);
            }
        }
        public Rect TexturCoord { get { return m_rcTextureCoord; } }
        void AttachTextureRes(Texture tr)
        {
            m_Texture = tr;
            int ic = m_iTileIndex / m_iTileX;
            int ir = m_iTileIndex - ic * m_iTileX;
            m_rcTextureCoord.xMin = (float)ic/ (float)m_iTileX;
            m_rcTextureCoord.yMin = (float)ir/ (float)m_iTileY;
            m_rcTextureCoord.xMax = (float)(ic+1.0f) / (float)m_iTileX;
            m_rcTextureCoord.yMax = (float)(ir+1.0f) / (float)m_iTileY;


            if ((m_Flip & ETextureFlip.Vertically) != 0)
            {
                float fBot = m_rcTextureCoord.yMax;
                m_rcTextureCoord.yMax = m_rcTextureCoord.yMin;
                m_rcTextureCoord.yMin = fBot;
            }
            if ((m_Flip & ETextureFlip.Horizontally) != 0)
            {
                float fRight = m_rcTextureCoord.xMax;
                m_rcTextureCoord.xMax = m_rcTextureCoord.xMin;
                m_rcTextureCoord.xMin = fRight;
            }

            m_UVs[0] = new Vector2(m_rcTextureCoord.xMin, m_rcTextureCoord.yMax);
            m_UVs[1] = new Vector2(m_rcTextureCoord.xMax, m_rcTextureCoord.yMax);
            m_UVs[2] = new Vector2(m_rcTextureCoord.xMax, m_rcTextureCoord.yMin);
            m_UVs[3] = new Vector2(m_rcTextureCoord.xMin, m_rcTextureCoord.yMin);
         
        }
         
#if UNITY_EDITOR
 
     
#endif

    }

}
