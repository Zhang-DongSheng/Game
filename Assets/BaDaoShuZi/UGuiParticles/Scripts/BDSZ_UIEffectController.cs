using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace BDSZ_2020
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
 
    public class BDSZ_UIEffectController : MonoBehaviour
    {


        protected List<BDSZ_EffectBase> m_Effects = new List<BDSZ_EffectBase>();
        protected float m_fLivingTime = 0.0f;
        protected bool m_bActive = true;
      
        protected BDSZ_EffectMoveProxy m_MoveProxy = null;

       
        public bool IsActive() { return m_bActive; }

        public void SetPosition(Vector3 vPosition)
        {
            gameObject.transform.position = vPosition;
        }
      
        public void SetActive(bool bAcitve, bool bResetStartTime)
        {
            m_bActive = bAcitve;

            gameObject.SetActive(m_bActive);
            if (bResetStartTime == true)
            {
                m_fLivingTime = 0.0f;
                for (int i = 0; i < m_Effects.Count; i++)
                {
                    m_Effects[i].Reset();
                }
            }
        }
        public void AddEffect(BDSZ_EffectBase eb)
        {
            if (m_Effects.Contains(eb) == false)
            {
                eb.SetMoveProxy(m_MoveProxy);
                m_Effects.Add(eb);
            }
        }
        public void RemoveEffect(BDSZ_EffectBase eb)
        {
            m_Effects.Remove(eb);
        }

        public int GetEmitterCount()
        {
            if (m_Effects == null)
                return 0;
            return m_Effects.Count;
        }
        public BDSZ_EffectBase GetEmitter(int iIndex)
        {
            return m_Effects[iIndex];
        }
        protected void Start()
        {
            GetComponent();

            for (int i = 0; i < m_Effects.Count; i++)
            {
                m_Effects[i].SetMoveProxy(m_MoveProxy);
                m_Effects[i].Start();
            }
        }
        public void Reset(bool bResetStartTime)
        {
            if (m_MoveProxy != null)
            {
                m_MoveProxy.Reset();
            }
            if (bResetStartTime == true)
            {
                m_fLivingTime = 0.0f;
            }
            for (int i = 0; i < m_Effects.Count; i++)
            {
                m_Effects[i].Reset();
            }
        }
        void OnDestroy()
        {

        }
        public bool UpdateEffect(Rect rc)
        {
            if (m_bActive == false)
                return false;
            m_fLivingTime += Time.deltaTime;
            if (m_MoveProxy != null)
            {
                m_MoveProxy.UpdateEffect(m_fLivingTime, Time.deltaTime);
            }
            
            int iLivingEmitterCount = 0;
            for (int i = 0; i < m_Effects.Count; i++)
            {
                if (m_Effects[i].IsEnd)
                    continue;
                float ft = m_fLivingTime - m_Effects[i].StartDelay;
                if (ft >= 0.0f)
                {
                    m_Effects[i].UpdateEffect(rc,ft, Time.deltaTime);
                }
                iLivingEmitterCount++;
            }
            m_bActive = iLivingEmitterCount > 0;
            return true;
        }

       
        void GetComponent()
        {
            m_MoveProxy = gameObject.GetComponentInChildren<BDSZ_EffectMoveProxy>();
            if (m_MoveProxy != null)
            {
                m_MoveProxy.Start();
            }

        }
        public void Draw(BDSZ_ParticleRoot uc)
        {
            for (int i = 0; i < m_Effects.Count; i++)
            {
                if (m_Effects[i].IsEnd)
                    continue;
                m_Effects[i].Render2D(uc);
            }
 
        }
        public void OnEnable()
        {
            Reset(true);
            if (transform.parent != null)
            {
                BDSZ_ParticleRoot parent = transform.parent.gameObject.GetComponent<BDSZ_ParticleRoot>();
                if (parent != null)
                {
                    parent.AddEffect(this);
                }
            }

            GetComponent();
            EnsureChildOrder();
            SetActive(true, true);
        }
        void CollectChild(Transform tf)
        {
            for (int i = 0; i < tf.childCount; i++)
            {
                BDSZ_EffectBase efBase = tf.GetChild(i).GetComponent<BDSZ_EffectBase>();
                if (efBase != null)
                {
                    AddEffect(efBase);
                }
                CollectChild(tf.GetChild(i));
            }

        }
        protected void EnsureChildOrder()
        {
            m_Effects.Clear();
            CollectChild(transform);
        }
#if UNITY_EDITOR
        protected void DrawWireframeRect(BDSZ_CanvasDraw draw, Rect rcDraw, float fWidth, float fz, Color32 uClr)
        {

            Rect rcCoord = new Rect(0, 0, 1.0f, 1.0f);
            Rect rcTop = new Rect(rcDraw.xMin, rcDraw.yMin, rcDraw.width, fWidth);
            draw.DrawQuadrangle(rcTop, rcCoord, fz, uClr);
            rcTop = new Rect(rcDraw.xMin, rcDraw.yMax - fWidth, rcDraw.width, fWidth);
            draw.DrawQuadrangle( rcTop,  rcCoord, fz, uClr);

            Rect rcVertical = new Rect(rcDraw.xMin, rcDraw.yMin, fWidth, rcDraw.height);
            draw.DrawQuadrangle( rcVertical,  rcCoord, fz, uClr);
            rcVertical = new Rect(rcDraw.xMax - fWidth, rcDraw.yMin, fWidth, rcDraw.height);
            draw.DrawQuadrangle(rcVertical, rcCoord, fz, uClr);
        }
        public void DrawSelectedBounds(Rect rcClient)
        {

            BDSZ_CanvasDraw draw = BDSZ_UIParticlesResMgr.GetSelectBoundRenderer(gameObject);
            if (draw == null)
                return;
            DrawWireframeRect(draw, rcClient, 1.0f, 0.0f, Color.green);
            draw.EndDraw(null);
        }
        public virtual void OnValidate()
        {
            if (transform.parent != null)
            {
                BDSZ_ParticleRoot parent = transform.parent.gameObject.GetComponent<BDSZ_ParticleRoot>();
                if (parent != null)
                {
                    parent.SetVerticesDirty();
                }
            }
        }
       
     
#endif
    }
}