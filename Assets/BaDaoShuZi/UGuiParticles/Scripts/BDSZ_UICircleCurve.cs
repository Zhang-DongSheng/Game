using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDSZ_2020
{
    public class BDSZ_UICircleCurve : BDSZ_UICurveBase
    {
         
        //需要保存的数据 ================================================
        [SerializeField]
        [Range(0.1f,10.0f)]
        protected float m_fCircleRadius=0.5f;//圆半径//
        [SerializeField]
        [Range(1,360)]
        protected int m_iArcAngle=360;//圆弧角度//
        //需要保存的数据 ================================================
        protected override void UpdateCurveData()
        {
            float fr = m_fCircleRadius;
            m_fCurveLength = Mathf.PI * 2.0f * fr * m_iArcAngle / 360.0f;
        }
        public override bool GetInterpolation(float fDistance, ref Vector2 vPosition, ref Vector2 vTangent)
        {
            float fAngle = fDistance /m_fCircleRadius;
            vTangent.x = Mathf.Cos(fAngle);
            vTangent.y = -Mathf.Sin(fAngle);
            vPosition.x = -m_fCircleRadius * vTangent.y;
            vPosition.y =  m_fCircleRadius * vTangent.x;
            return true;
        }
 
    }
}