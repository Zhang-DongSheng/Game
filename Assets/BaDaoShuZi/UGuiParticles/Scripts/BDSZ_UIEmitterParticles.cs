
using System;
using System.Collections.Generic;
using UnityEngine;


namespace BDSZ_2020
{
    [ExecuteInEditMode]
    public class BDSZ_UIEmitterParticles : BDSZ_UIParticle
    {
        //需要保存的数据==================================================
        [SerializeField]
        BDSZ_EffectEmitter m_Emitter = BDSZ_EffectEmitter.defaultEmitter;
        //需要保存的数据==================================================

        protected StUIParticleDef m_ParticleHead;

        public override void Start()
        {
            m_bEmitter = true;
            m_Emitter.Start();
            base.Start();

        }
        //释放粒子//
        public void FreeParticle()
        {

            while (m_ParticleHead != null)
            {
                StUIParticleDef pNext = m_ParticleHead.Next;
                BDSZ_UIParticlesResMgr.Instance.FreeParticle(m_ParticleHead);
                m_ParticleHead = pNext;
            }
            m_Emitter.Clean();

        }
        public override void Reset()
        {
            base.Reset();
            FreeParticle();
            m_Emitter.Reset();
        }


        public void InitEmitterParticle(StUIParticleDef pParticle,Rect rc, float fDeltaTime)
        {
            m_Emitter.Emitter2D(rc, ref pParticle.vCurrentPosition, ref pParticle.vVelocityDir);
            if (IsEffectAttribute(EEffectAttribute.StretchedBillboard ))
            {
                pParticle.vYAxis = pParticle.vVelocityDir;
            }
            else
            {
                pParticle.vYAxis = Vector3.up;
                pParticle.vXAxis = Vector3.right;
            }
        }
        void UpdateEmitterParticle(float fUpdateTime, float fDeltaTime)
        {
           
            StUIParticleDef p = m_ParticleHead;
            while (p != null)
            {
                UpdateParticle(p, fUpdateTime, fDeltaTime);
                p = p.Next;
            }
        }
        public override void Render2D(BDSZ_ParticleRoot uc)
        {
            BDSZ_CanvasDraw rd = uc.FindCanvasRender(m_Material, 0);
            StUIParticleDef p = m_ParticleHead;
            while (p != null)
            {

                RenderParticle(rd, uc, p);
                p = p.Next;
            }
#if UNITY_EDITOR
            if (BDSZ_UIParticlesResMgr.IsSelected(gameObject))
            {
                DrawSelectBound();
            }
#endif
        }

        public override void UpdateEffect(Rect rc, float fUpdateTime, float fDeltaTime)
        {
             base.UpdateEffect(rc,fUpdateTime, fDeltaTime);
            //首先释放到达生命周期的粒子===============================
            StUIParticleDef pBefore = null;
            StUIParticleDef p = m_ParticleHead;
            while (p != null)
            {
                StUIParticleDef pNext = p.Next;
                if (p.fEndTime <= fUpdateTime)
                {
                    BDSZ_UIParticlesResMgr.Instance.FreeParticle(p);
                    if (pBefore != null)
                        pBefore.Next = pNext;
                    else
                        m_ParticleHead = pNext;
                    m_Emitter.DecEmitterCount();
                }
                else
                {

                    pBefore = p;
                }
                p = pNext;
            }

            //首先释放到达生命周期的粒子===============================

            //准备产生新的粒子=========================
            int iNewParticle = m_Emitter.GetEmitterCount( fDeltaTime, IsEffectAttribute(EEffectAttribute.Looping));
            //Debug.LogFormat("iNewParticle = {0} fDeltaTime = {1}", iNewParticle, fDeltaTime);
            for (int i = 0; i < iNewParticle; i++)
            {
                //AppLogInfo(_T(" Time=%d iNewParticle=%d "),dwTime,iNewParticle);
                p = BDSZ_UIParticlesResMgr.Instance.GetFreeParticle();
                if (p != null)
                {
                    p.fStartTime = fUpdateTime;
                    p.fEndTime = p.fStartTime + m_fDuration;

                    p.iTexCoordIndex = m_TextureRes.GetCellIndex();

                    p.vYAxis = Vector3.up;
                    p.vXAxis = Vector3.left;
                    InitEmitterParticle(p,rc, fDeltaTime);
                    InitParticle(p, fDeltaTime);
                    p.Next = m_ParticleHead;
                    m_ParticleHead = p;
                    m_Emitter.IncEmitterCount();
                }
            }
            //准备产生新的粒子=========================
            UpdateEmitterParticle(fUpdateTime, fDeltaTime);
        }
#if UNITY_EDITOR
        public override void DrawSelectBound()
        {

            if (m_Controller == null)
                return;
            BDSZ_ParticleRoot pr = m_Controller.GetComponentInParent<BDSZ_ParticleRoot>();
            Rect rcArea = pr.GetPixelAdjustedRect();
            Vector2 vSize = m_Emitter.GetExtent();
            Vector2 vAreaScale = new Vector2(rcArea.width * vSize.x, rcArea.height * vSize.y);
            Vector2 vPosition = new Vector2(m_vPosition.x * rcArea.width, m_vPosition.y * rcArea.height) + new Vector2(rcArea.xMin, rcArea.yMin);
            m_Controller.DrawSelectedBounds(new Rect(vPosition-vAreaScale*0.5f, vAreaScale));
        }

        public override void OnValidate()
        {
            Reset();
            base.OnValidate();
        }
#endif
    }
}
