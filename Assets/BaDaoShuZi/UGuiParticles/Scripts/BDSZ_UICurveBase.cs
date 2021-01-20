using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDSZ_2020
{

    public class BDSZ_UICurveBase : MonoBehaviour
    {

        protected float m_fCurveLength = 0.0f;
        public float CurveLength { get { return m_fCurveLength; } }
        public virtual bool GetInterpolation(float fDistance, ref Vector2 vPosition, ref Vector2 vTangent)
        {
            return false;
        }

        protected virtual void UpdateCurveData()
        {
        }
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            UpdateCurveData();
        }
#endif
    }
}