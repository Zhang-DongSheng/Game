 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDSZ_2020
{
    public class BDSZ_EffectWindForce : BDSZ_EffectMoveProxy
    {

 
     
        [SerializeField]
        EffectFloatMinMax m_AngleFrequency = new EffectFloatMinMax(0.2f);
        [SerializeField]
        EffectFloatMinMax m_AngleRange = new EffectFloatMinMax(60.0f);
        [SerializeField]
        EffectFloatMinMax m_ForceFrequency = new EffectFloatMinMax(0.2f);
        [SerializeField]
        EffectFloatMinMax m_ForceRange = new EffectFloatMinMax(0.2f);
        [SerializeField]
        EffectFloatMinMax m_ParticleInertia = new EffectFloatMinMax(1.0f);
        [SerializeField]

        [Range(-100.0f, 100.0f)]
        protected float m_fGravity = -1.0f;
        [SerializeField]
        Vector2 m_vBoundMin = new Vector2(-10.0f,-10.0f);
        [SerializeField]
        Vector2 m_vBoundMax = new Vector2(10.0f,10.0f);
        //==============================================================
        float m_fBoundMagnitude = 1.0f;
        Vector4 m_vYawData;
        Vector4 m_vForceData;
        Vector2 m_vAccelerated;

        public override void Reset()
        {
            base.Reset();
            InitData();
        }
        void InitData()
        {
            m_vYawData.x = Random.Range(m_AngleRange.Min,m_AngleRange.Max);
            m_vForceData.x = Random.Range(m_ForceRange.Min, m_ForceRange.Max);
            m_vForceData.w = 0;
            m_vYawData.w = 0.0f;
            RandData(ref m_vYawData,m_AngleRange,m_AngleFrequency);
            RandData(ref m_vForceData,m_ForceRange, m_ForceFrequency);
            m_fBoundMagnitude = Mathf.Max(0.01f, (m_vBoundMax - m_vBoundMin).magnitude);
            
        }
        public override void Start()
        {
            base.Start();
            InitData();
        }

        void RandData(ref Vector4 v, EffectFloatMinMax range, EffectFloatMinMax time)
        {
            float fa = range.GetValue();
            float ft = time.GetValue();
            float fNewA =Mathf.Clamp(v.x + fa,range.Min,range.Max);
            fa = fNewA - v.x;
            v.y = fa / ft;
            v.z = v.w;
            v.w += ft;
        }

       
        public override void OnInitParticle(StUIParticleDef p)
        {
            p.CustomDataFloat[0] = m_ParticleInertia.GetValue();
 
        }

        public override void UpdateEffect(float fLifeTime, float fDeltaTime)
        {
            base.UpdateEffect(fLifeTime, fDeltaTime);
          
            if (fLifeTime >= m_vYawData.w)
            {
                RandData(ref m_vYawData, m_AngleRange, m_AngleFrequency);
            }
            if (fLifeTime>=m_vForceData.w)
            {
                RandData(ref m_vForceData, m_ForceRange, m_ForceFrequency );
            }

            m_vYawData.x += m_vYawData.y * fDeltaTime;
            m_vForceData.x += m_vForceData.y * fDeltaTime;
            Vector2 vYAxis = Vector2.up;//  MathUtility.DirFromPitchYaw(m_vPitchData.x, m_vYawData.x);
            float fAngle = Mathf.Deg2Rad * m_vYawData.x;
            vYAxis.y =0.0f- Mathf.Cos(fAngle);
            vYAxis.x = Mathf.Sin(fAngle);
           // vYAxis ;
            vYAxis.y += m_fGravity;
            vYAxis.Normalize();
            m_vAccelerated = vYAxis * m_vForceData.x;
 
            
        }
        public override void UpdateMove(StUIParticleDef p, float fUpdateAlpha, float fDeltaTime)
        {
            Vector2 vNewVelocity = p.fVelocity * p.vVelocityDir + m_vAccelerated * fDeltaTime*p.CustomDataFloat[0];


            p.vCurrentPosition += vNewVelocity * fDeltaTime;
            p.fVelocity = vNewVelocity.magnitude;
            p.vVelocityDir = vNewVelocity.normalized;
            Vector2 vAdjustDir = p.vVelocityDir;
            if (p.vCurrentPosition.x < m_vBoundMin.x)
            {
                vAdjustDir.x = (m_vBoundMax.x - p.vCurrentPosition.x) / m_fBoundMagnitude;
            }
            else if (p.vCurrentPosition.x > m_vBoundMax.x)
            {
                vAdjustDir.x = (m_vBoundMin.x - p.vCurrentPosition.x) / m_fBoundMagnitude;
            }
            if (p.vCurrentPosition.y < m_vBoundMin.y)
            {
                vAdjustDir.y = (m_vBoundMax.y - p.vCurrentPosition.y) / m_fBoundMagnitude;
            }
            else if (p.vCurrentPosition.y > m_vBoundMax.y)
            {
                vAdjustDir.y = (m_vBoundMin.y - p.vCurrentPosition.y) / m_fBoundMagnitude;
            }

            Vector2 vSub = p.vVelocityDir - vAdjustDir;
            const float epsilon = 0.0001f;
            if (Mathf.Abs(vSub.x)>= epsilon || Mathf.Abs(vSub.y)>= epsilon)
            {
                vAdjustDir.Normalize();
                p.vVelocityDir = Vector3.Slerp(p.vVelocityDir, vAdjustDir, fDeltaTime);
            }



        }
#if UNITY_EDITOR
        public override void  OnValidate()
        {
            InitData();
        }
 
        public EffectFloatMinMax GetAngleRange() { return m_AngleRange; }
        public void SetAngleRange(EffectFloatMinMax value) { m_AngleRange = value; }
        public EffectFloatMinMax GetAngleFrequency() { return m_AngleFrequency; }
        public void SetAngleFrequency(EffectFloatMinMax value) { m_AngleFrequency = value; }

        public EffectFloatMinMax GetForceRange() { return m_ForceRange; }
        public void SetForceRange(EffectFloatMinMax value) { m_ForceRange = value; }

        public EffectFloatMinMax GetForceFrequency() { return m_ForceFrequency; }
        public void SetForceFrequency(EffectFloatMinMax value) { m_ForceFrequency = value; }

        public EffectFloatMinMax GetParticleInertia() { return m_ParticleInertia; }
        public void SetParticleInertia(EffectFloatMinMax value) { m_ParticleInertia = value; }
#endif
    }
}