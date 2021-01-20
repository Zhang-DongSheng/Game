using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDSZ_2020
{
    public class BDSZ_UIRectCurve : BDSZ_UICurveBase
    {
         
        //需要保存的数据 ================================================
        [SerializeField]
        [Range(0.1f,10.0f)]
        protected float m_fWidth=1.0f;
        [SerializeField]
        [Range(0.1f, 10.0f)]
        protected float m_fHeight = 1.0f;
        //需要保存的数据 ================================================
        protected override void UpdateCurveData()
        {
            m_fCurveLength = (m_fWidth + m_fHeight) * 2.0f;
        }
        public override bool GetInterpolation(float fDistance, ref Vector2 vPosition, ref Vector2 vTangent)
        {
            float fClamp = fDistance % m_fCurveLength;
            if (fClamp > m_fWidth * 2.0f + m_fHeight)
            {
                vPosition.y = (fClamp - (m_fWidth * 2.0f + m_fHeight))- m_fHeight * 0.5f ;
                vPosition.x = -m_fWidth * 0.5f;
                vTangent = new Vector2(-1.0f, 0.0f);
            }
            else if (fClamp > m_fWidth + m_fHeight)
            {
                vPosition.x = m_fWidth * 0.5f - (fClamp - (m_fWidth + m_fHeight));
                vPosition.y = -m_fHeight * 0.5f;
                vTangent = new Vector2(0.0f, -1.0f);
            }
            else if (fClamp > m_fWidth)
            {
                vPosition.y = m_fHeight * 0.5f - (fClamp - m_fWidth);
                vPosition.x = m_fWidth * 0.5f;
                vTangent = new Vector2(1.0f, 0.0f);
            }
            else
            {
                vPosition.x = fClamp - m_fWidth * 0.5f;
                vPosition.y = m_fHeight * 0.5f;
                vTangent = new Vector2(0.0f, 1.0f);
            }
            return true;
        }

    }
}