using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace BDSZ_2020
{

    [ExecuteInEditMode]

#if UNITY_EDITOR
    public class BDSZ_EffectBase : BDSZ_EffectBehaviour
#else
    public class BDSZ_EffectBase : MonoBehaviour
#endif
    {
        //需要保存的数据==================================================
        [SerializeField]
        protected EEffectShaderType m_ShaderType = EEffectShaderType.Transparent;
        [SerializeField]
        protected BDSZ_EffectTextureRes m_TextureRes = BDSZ_EffectTextureRes.defaultRes;
        [SerializeField]
        protected float m_fDuration = 5.0f;
        [SerializeField]
        protected float m_fStartDelay = 0.0f;
        [SerializeField]
        protected int m_iEffectAttribute = 0;
        [SerializeField]
        protected EffectColorMinMax m_StartColor = new EffectColorMinMax(Color.white);
        [SerializeField]
        protected EffectColorGradient m_UpdateColor = new EffectColorGradient(Color.white);
        [SerializeField]
        [Range(0.0f, 1.0f)]
        protected float m_fFadeIn = 0.0f;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        protected float m_fFadeOut = 1.0f;
        [SerializeField]
        protected int m_iUpdateAttribute = 0;
        [SerializeField]
        protected Vector2 m_vPosition = new Vector2(0.5f, 0.5f);
        [SerializeField]
        protected float m_fCurveVelocity = 1.0f;//曲线运行速度。//
                                                //需要保存的数据==================================================
        protected EEffectBehaviourState m_State = EEffectBehaviourState.None;
        protected Material m_Material = null;
        public bool IsEnd { get { return m_State == EEffectBehaviourState.End; } }

        protected int m_iMaterialIndex = 0;
        protected BDSZ_UICurveBase m_Curve = null;
        protected BDSZ_EffectMoveProxy m_MoveProxy = null;
        protected BDSZ_UIEffectController m_Controller = null;
        protected bool m_bCreate = false;


        public virtual void Start()
        {

            m_State = EEffectBehaviourState.Playing;
            UpdateAttribute();
            m_bCreate = true;
            UpdateData();
            m_Controller = GetController();
            if (m_Controller != null)
            {
                m_Controller.AddEffect(this);
            }
            UpdateMaterial(false);
        }

        public void SetMainTexture(Texture texture)
        {
            m_TextureRes.MainTexture = texture;
        }
        public virtual void Reset()
        {

        }
        public virtual void UpdateEffect(Rect rc, float fLifeTime, float fDeltaTime)
        {

        }


        public void AddEffectAttribute(EEffectAttribute iAttr)
        {
            m_iEffectAttribute |= (int)iAttr;
        }
        public void RemoveEffectAttribute(EEffectAttribute iAttr)
        {
            m_iEffectAttribute &= (~(int)iAttr);
        }
        public void UpdateEffectAttribute(bool bEnable, EEffectAttribute iAttr)
        {
            if (bEnable)
                AddEffectAttribute(iAttr);
            else
                RemoveEffectAttribute(iAttr);
        }
        public bool IsEffectAttribute(EEffectAttribute iAttr)
        {
            return (m_iEffectAttribute & (int)iAttr) != 0;
        }
        //材质属性=============================

        protected virtual void UpdateAttribute()
        {
            ModifyUpdateAttribute(m_UpdateColor.IsConstant() == false, EEffectUpdateAttribute.UpdateColor);
            ModifyUpdateAttribute((m_fFadeIn != 0.0f || m_fFadeOut != 1.0f), EEffectUpdateAttribute.UpdateAlpha);

        }
        public void AddUpdateAttribute(EEffectUpdateAttribute iAttr)
        {
            m_iUpdateAttribute |= (int)iAttr;
        }
        public void RemoveUpdateAttribute(EEffectUpdateAttribute iAttr)
        {
            m_iUpdateAttribute &= (~(int)iAttr);
        }
        public void ModifyUpdateAttribute(bool bEnable, EEffectUpdateAttribute iAttr)
        {
            if (bEnable)
                AddUpdateAttribute(iAttr);
            else
                RemoveUpdateAttribute(iAttr);
        }
        public bool IsUpdateAttribute(EEffectUpdateAttribute iAttr)
        {
            return (m_iUpdateAttribute & (int)iAttr) != 0;
        }
        public float StartDelay { get { return m_fStartDelay; } }

        protected void UpdateColor(StUIParticleDef p, float fUpdateA, float fLifeAlpha)
        {
            
            if (IsUpdateAttribute(EEffectUpdateAttribute.UpdateColor))
            {
                p.dwColor = p.OriginColor * m_UpdateColor.Evaluate(fUpdateA);
            }
            else
            {
                p.dwColor = p.OriginColor;
            }
            if (IsUpdateAttribute(EEffectUpdateAttribute.UpdateAlpha))
            {
                float fAlpha = 1.0f;
                if (fLifeAlpha < m_fFadeIn)
                    fAlpha = fLifeAlpha / m_fFadeIn;
                else if (fLifeAlpha > m_fFadeOut)
                    fAlpha = 1.0f - (fLifeAlpha - m_fFadeOut) / (1.0f - m_fFadeOut);
                if (fAlpha != 1.0f)
                {
                    p.dwColor.a = p.dwColor.a * fAlpha;
                }
            }
            if (m_ShaderType == EEffectShaderType.ColorAdditive)
            {
                if (p.dwColor.a < 1.0f)
                {
                    p.dwColor.r = p.dwColor.a * p.dwColor.r;
                    p.dwColor.g = p.dwColor.a * p.dwColor.g;
                    p.dwColor.b = p.dwColor.a * p.dwColor.b;
                }
            }
             
        }
        public void SetMoveProxy(BDSZ_EffectMoveProxy ep)
        {
            m_MoveProxy = ep;
        }
        public virtual void OnDestroy()
        {
            if (m_Material != null)
            {
                BDSZ_UIParticlesResMgr.ReleaseMaterial(m_Material, m_iMaterialIndex);
                m_Material = null;
            }
            if (m_Controller != null)
            {
                m_Controller.RemoveEffect(this);
            }
        }

        public virtual void UpdateData()
        {

            UpdateAttribute();
            GetComponent();
        }


        public virtual void Render2D(BDSZ_ParticleRoot uc)
        {

        }

        void GetComponent()
        {
            if (gameObject != null && gameObject.transform.parent != null)
            {
                m_Curve = gameObject.transform.parent.GetComponentInChildren<BDSZ_UICurveBase>();

            }
        }
        public virtual void UpdateMaterial(bool bForce)
        {

        }
        public BDSZ_UIEffectController GetController()
        {
            BDSZ_UIEffectController controller = null;
            if (m_Controller != null)
                return m_Controller;
            Transform tf = gameObject.transform.parent;
            while (tf != null)
            {
                controller = tf.GetComponent<BDSZ_UIEffectController>() as BDSZ_UIEffectController;
                if (controller != null)
                    break;
                tf = tf.parent;
            }
            return controller;
        }
        public BDSZ_UICurveBase Curve { get { return m_Curve; } }
#if UNITY_EDITOR
        public void OnTransformParentChanged()
        {
            GetComponent();
        }


        public override void OnValidate()
        {
            UpdateAttribute();

            if (m_bCreate == true)
            {
                UpdateData();
            }
            UpdateMaterial(true);
        }


        public virtual void OnEditorCreate()
        {
            AddEffectAttribute(EEffectAttribute.Looping);
            Start();
        }
        public EffectColorMinMax GetStartColor() { return m_StartColor; }
        public void SetStartColor(EffectColorMinMax value) { m_StartColor = value; }
        public EffectColorGradient GetUpdateColor() { return m_UpdateColor; }
        public void SetUpdateColor(EffectColorGradient value) { m_UpdateColor = value; }
#endif
    }
}