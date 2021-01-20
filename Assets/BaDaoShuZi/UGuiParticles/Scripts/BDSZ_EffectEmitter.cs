using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BDSZ_2020
{
 
    [Serializable]
 
    public class BDSZ_EffectEmitter
    {
        //需要保存的数据==================================================
        [SerializeField]
        EEmitterType m_EmitterType = EEmitterType.Circle;
        [SerializeField]
        protected bool m_bInnerEmitter = true;
        [SerializeField]
        protected bool m_bRandDir = false;
        [SerializeField]
        protected float m_fEmitterRatio = 10.0f;
        [SerializeField]
        [Range(1, 1000)]
        protected int m_iEmitterMax = 100;
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        protected float m_fEmitterCycle = 0.0f;
        [SerializeField]
        protected Vector2 m_vExtent = new Vector2(1.0f, 1.0f);
        [SerializeField]
        // [Range(0.1f, 100.0f)]
        protected float m_fSphereInnerRadius = 0.0f;
        [SerializeField]
        // [Range(0.1f, 100.0f)]
        protected float m_fSphereOuterRadius = 1.0f;

        [SerializeField]
        [Range(0.0f, 90.0f)]
        protected float m_fConeAngle = 30.0f;
        [SerializeField]
        [Range(-180.0f, 180.0f)]
        protected float m_fEmiterDirAngle = 0.0f;


        //需要保存的数据==================================================
        protected Vector2 m_vEmitterYAxis = Vector2.up;
        protected Vector2 m_vEmitterXAxis = Vector2.right;
        protected float m_fLastEmitterTime = 0.0f;
        protected int m_iActiveEmmiterCount = 0;
        protected int m_iHasEmitterCount = 0;
        protected float m_fEmitterCountFraction = 0.0f;


        public int EmitterMax { get { return m_iEmitterMax; } }
        public void IncEmitterCount()
        {
            m_iHasEmitterCount++;
            m_iActiveEmmiterCount++;
        }
        public void DecEmitterCount()
        {
            m_iActiveEmmiterCount--;
        }
        public bool RandDirection { get { return m_bRandDir; } }
        public static BDSZ_EffectEmitter defaultEmitter
        {
            get
            {
                var emitter = new BDSZ_EffectEmitter
                {

                };
                return emitter;
            }
        }


        public void Start()
        {


            float fAngle = m_fEmiterDirAngle * Mathf.Deg2Rad;
            m_vEmitterYAxis.y = Mathf.Cos(fAngle);
            m_vEmitterYAxis.x = Mathf.Sin(fAngle);
            m_vEmitterXAxis = new Vector2(m_vEmitterYAxis.y, -m_vEmitterYAxis.x);
        }
        //释放粒子//
        public void Clean()
        {
            m_iActiveEmmiterCount = 0;
            m_iHasEmitterCount = 0;

        }
        public void Reset()
        {
            m_fLastEmitterTime = 0.0f;
            m_fEmitterCountFraction = 0.0f;
        }
        //返回当前需要发射的个数。
        public int GetEmitterCount(float fDeltaTime, bool bLooping)
        {
            float fEmitterTime = fDeltaTime;
            if (m_fEmitterCycle > 0.0f)
            {
                m_fLastEmitterTime -= fDeltaTime;
                if (m_fLastEmitterTime > 0.0f)
                    return 0;
            }
            if (m_iActiveEmmiterCount >= m_iEmitterMax)
                return 0;
            if (m_fEmitterCycle > 0.0f)
            {
                m_fLastEmitterTime = m_fEmitterCycle;
                fEmitterTime = m_fEmitterCycle;
            }
            if (bLooping == false && m_iHasEmitterCount >= m_iEmitterMax)
            {
                return 0;
            }

            if (m_fEmitterRatio == 0)
            {
                return m_iEmitterMax - m_iActiveEmmiterCount;
            }
            else
            {
                float fEmitterCount = fEmitterTime * m_fEmitterRatio + m_fEmitterCountFraction;
                int iEmitterCount = Mathf.FloorToInt(fEmitterCount);
                m_fEmitterCountFraction = fEmitterCount - iEmitterCount;
                return Mathf.Min(iEmitterCount, m_iEmitterMax - m_iActiveEmmiterCount);
            }
        }

        Vector2 RandCircleDirection()
        {

            float fAngle = UnityEngine.Random.Range(0, 360.0f) * Mathf.Deg2Rad;
            Vector2 vDir = new Vector2(Mathf.Sin(fAngle), Mathf.Cos(fAngle));
            return vDir;
        }
        public void Emitter2D(Rect rc, ref Vector2 vPos, ref Vector2 vVelocityDir)
        {
            vVelocityDir = m_vEmitterYAxis;
            if (m_EmitterType == EEmitterType.Circle)
            {
                Vector2 vRandDir = RandCircleDirection();
                if (m_bRandDir)
                {
                    if (m_fSphereInnerRadius > m_fSphereOuterRadius)
                    {
                        vVelocityDir = -vRandDir;
                    }
                    else
                    {
                        vVelocityDir = vRandDir;
                    }
                }
                if (m_bInnerEmitter == false)
                {
                    vPos = m_fSphereOuterRadius * vRandDir;
                }
                else
                {
                    float f = UnityEngine.Random.Range(m_fSphereInnerRadius, m_fSphereOuterRadius);
                    vPos = f * vRandDir;
                }
            }
            else if (m_EmitterType == EEmitterType.Rect)
            {
                vPos.x = UnityEngine.Random.Range(-m_vExtent.x, m_vExtent.x)*0.5f;
                vPos.y = UnityEngine.Random.Range(-m_vExtent.y, m_vExtent.y)*0.5f;
                if (m_bInnerEmitter == false)
                {
                     Vector2 vRealExtent = new Vector2(m_vExtent.x * rc.width, m_vExtent.y * rc.height) * 0.5f;
                     float fPosFactor = UnityEngine.Random.Range(0, (vRealExtent.x+ vRealExtent.y));
                     if (fPosFactor> vRealExtent.x+ vRealExtent.y*0.5f)
                    {
                        vPos.x = -m_vExtent.x*0.5f;
                    }
                    else if (fPosFactor>(vRealExtent.x+ vRealExtent.y)*0.5f)
                    {
                        vPos.y = -m_vExtent.y*0.5f;
                    }
                    else if (fPosFactor> vRealExtent.x*0.5f)
                    {
                        vPos.x = m_vExtent.x*0.5f;
                    }
                    else 
                    {
                        vPos.y = m_vExtent.y*0.5f;
                    }

                }
            }
            else if (m_EmitterType == EEmitterType.Cone)
            {

                vVelocityDir = Mathf.Sin(UnityEngine.Random.Range(-m_fConeAngle, m_fConeAngle) * Mathf.Deg2Rad) * m_vEmitterXAxis;
                vVelocityDir += m_vEmitterYAxis * Mathf.Sqrt(1.0f - vVelocityDir.x * vVelocityDir.x);
                vPos.x = m_vExtent.x * vVelocityDir.x;
                vPos.y = m_vExtent.y * vVelocityDir.y;
            }
        }



        public EEmitterType EmitterType { get { return m_EmitterType; } set { m_EmitterType = value; } }
        public Vector2 Extent { get { return m_vExtent; } set { m_vExtent = value; } }
#if UNITY_EDITOR
        public Vector2 GetExtent()
        {
            if (m_EmitterType == EEmitterType.Circle)
                return new Vector2(m_fSphereOuterRadius, m_fSphereOuterRadius);
            else
                return m_vExtent;
            
        }
        public float InnerRadius { get { return m_fSphereInnerRadius; } set { m_fSphereInnerRadius = value; } }
        public float OuterRadius { get { return m_fSphereOuterRadius; } set { m_fSphereOuterRadius = value; } }
#endif
    }
}