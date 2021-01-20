
using System;
using System.Collections.Generic;
using UnityEngine;


namespace BDSZ_2020
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    //游戏中粒子数据//
    public class StUIParticleDef
    {
        public Vector2 vCurrentPosition;//粒子当前位置//
      
        public Vector2 vVelocityDir;//粒子当前速度//
        public Vector2 vYAxis;//y州//
        public Vector2 vXAxis;//x州//
        public float fCenterX;
        public float fCenterY;
        public float   fStartTime;//粒子开始时间。//
        public float   fEndTime;//粒子结束时间//
        public float   fCurrentAngle;//粒子当前角度//
        public float   fWidth; //粒子当前宽带//
        public float   fHeight;//粒子当前高度//
        public float   fOriginSize;
        public float   fOriginVelocity;
        public float   fVelocity;
        public float   fGravityVelocity;
        public Color   OriginColor;
        public Color   dwColor;//粒子颜色//
        public int     iTexCoordIndex;//粒子纹理坐标索引//
        public Vector4 CustomDataFloat;
        

        public StUIParticleDef Next;//下一个粒子//
    };
    public class BDSZ_UIParticle : BDSZ_EffectBase
    {
        //需要保存的数据==================================================
        [SerializeField]
        protected bool m_bAllowProxy=true;
        [SerializeField]
        protected EffectFloatMinMax m_StartSize = new EffectFloatMinMax(0.1f);
        [SerializeField]
        protected EffectFloatMinMax m_StartVelocity = new EffectFloatMinMax(0.2f);
       
        [SerializeField]
        protected EffectFloatMinMax m_StartAngle = new EffectFloatMinMax(0.0f);

        [SerializeField]
        protected EffectFloatMinMax m_fCenterX = new EffectFloatMinMax(0.5f);
        [SerializeField]
        protected EffectFloatMinMax m_fCenterY = new EffectFloatMinMax(0.5f);

        [SerializeField]
        protected EffectFloatGradient m_UpdateXScale = new EffectFloatGradient(1.0f);
        [SerializeField]
        protected EffectFloatGradient m_UpdateYScale = new EffectFloatGradient(1.0f);
        [SerializeField]
        protected EffectFloatGradient m_UpdateVelocity = new EffectFloatGradient(1.0f);
        [SerializeField]
        protected EffectFloatGradient m_UpdateAngle = new EffectFloatGradient(0.0f);
        [SerializeField]
        [Range(-100.0f, 100.0f)]
        protected float m_fGravity = 0.0f;
        [SerializeField]
        [Range(0.0f,3600.0f)]
        protected float m_fRunCycle = 0.0f;

        //需要保存的数据==================================================
        protected bool m_bEmitter = false;
        protected StUIParticleDef m_Particle = null;
        public override void UpdateEffect(Rect rc, float fUpdateTime, float fDeltaTime)
        {
            base.UpdateEffect(rc,fUpdateTime, fDeltaTime);
            if (m_bEmitter == false)
            {
                if (m_Particle == null)
                {
                    m_Particle = new StUIParticleDef
                    {
                        vYAxis = Vector2.up,
                        vXAxis = Vector2.right
                    };
                    InitParticle(m_Particle, 0.0f);
                }
                UpdateParticle(m_Particle, fUpdateTime, fDeltaTime);
            }
        }
        protected void UpdateParticle(StUIParticleDef pParticle, float fUpdateTime, float fDeltaTime)
        {
            float fpTime = fUpdateTime - pParticle.fStartTime;
            float fa =(fpTime / m_fDuration)%1.0f;
            float fUpdate = (m_fRunCycle == 0.0f) ? fa : ((fpTime / m_fRunCycle) % 1.0f);
            UpdateColor(pParticle,fUpdate, fa);
            
            if (IsUpdateAttribute(EEffectUpdateAttribute.UpdateAngle))
            {
                float fRotateVelocity = m_UpdateAngle.Evaluate(fUpdate);
                pParticle.fCurrentAngle += fRotateVelocity * fDeltaTime;
                if (pParticle.fCurrentAngle > 360.0f)
                    pParticle.fCurrentAngle -= 360.0f;
                else if (pParticle.fCurrentAngle < -360.0f)
                    pParticle.fCurrentAngle += 360.0f;
            }
            if (IsUpdateAttribute(EEffectUpdateAttribute.UpdateXScale))
            {
                float fXScale = m_UpdateXScale.Evaluate(fUpdate);
                pParticle.fWidth = pParticle.fOriginSize * fXScale;

            }
            if (IsUpdateAttribute(EEffectUpdateAttribute.UpdateYScale))
            {
                float fYScale = m_UpdateYScale.Evaluate(fUpdate);
                pParticle.fHeight = pParticle.fOriginSize * fYScale;
            }
            pParticle.iTexCoordIndex = m_TextureRes.UpdateCellIndex(fpTime, pParticle.iTexCoordIndex);
            pParticle.fGravityVelocity += m_fGravity * fDeltaTime;

            if (m_bAllowProxy && m_MoveProxy != null)
            {
                m_MoveProxy.UpdateMove(pParticle, fa, fDeltaTime);
            }
            else
            {
                if (IsUpdateAttribute(EEffectUpdateAttribute.UpdateVelocity))
                {
                    pParticle.fVelocity = pParticle.fOriginVelocity * m_UpdateVelocity.Evaluate(fa);
                }
                if (m_Curve != null)
                {
                    float fDistance = m_fCurveVelocity * fUpdateTime;
                    Vector2 vCurPos = Vector2.zero;
                    m_Curve.GetInterpolation(fDistance, ref vCurPos, ref pParticle.vYAxis);
                    pParticle.vXAxis = new Vector2(pParticle.vYAxis.y, -pParticle.vYAxis.x);
                    pParticle.vCurrentPosition = vCurPos;
                }
                else
                {
                    pParticle.vCurrentPosition += pParticle.fVelocity * pParticle.vVelocityDir * fDeltaTime;
                    pParticle.vCurrentPosition.y -= pParticle.fGravityVelocity * fDeltaTime;
                }
            }
        }
        protected void InitParticle(StUIParticleDef pParticle, float fDeltaTime)
        {

            pParticle.OriginColor = m_StartColor.GetValue();
            pParticle.dwColor = pParticle.OriginColor * m_UpdateColor.Evaluate(0);
            pParticle.fCenterX = m_fCenterX.GetValue();
            pParticle.fCenterY = m_fCenterY.GetValue();
            pParticle.fOriginVelocity = m_StartVelocity.GetValue();
            pParticle.fVelocity = pParticle.fOriginVelocity * m_UpdateVelocity.Evaluate(0);
            pParticle.fCurrentAngle = m_StartAngle.GetValue();
            pParticle.fOriginSize = m_StartSize.GetValue();
            pParticle.fWidth = pParticle.fOriginSize * m_UpdateXScale.Evaluate(0);
            pParticle.fHeight = pParticle.fOriginSize * m_UpdateYScale.Evaluate(0);
            
            pParticle.fGravityVelocity = 0.0f;
            if(m_bAllowProxy && m_MoveProxy!=null)
            {
                m_MoveProxy.OnInitParticle(pParticle);
            }
            

        }
      
        public override void UpdateData()
        {
            base.UpdateData();
            if(m_UpdateAngle.IsConstant() && m_UpdateAngle.MinValue==0.0f)
                RemoveUpdateAttribute(EEffectUpdateAttribute.UpdateAngle);
            else
                AddUpdateAttribute(EEffectUpdateAttribute.UpdateAngle);
          
            ModifyUpdateAttribute(m_UpdateVelocity.IsConstant() == false, EEffectUpdateAttribute.UpdateVelocity);
            ModifyUpdateAttribute(m_UpdateXScale.IsConstant() == false, EEffectUpdateAttribute.UpdateXScale);
            ModifyUpdateAttribute(m_UpdateYScale.IsConstant() == false, EEffectUpdateAttribute.UpdateYScale);
           
           
        }
        public override void UpdateMaterial(bool bForce)
        {
            int iNewIndex = (int)m_ShaderType;
            if (iNewIndex != m_iMaterialIndex || bForce)
            {
                if (m_Material != null)
                {
                    BDSZ_UIParticlesResMgr.ReleaseMaterial(m_Material, (int)m_ShaderType);
                    m_Material = null;
                }
            }
            if (m_Material == null)
            {
                m_iMaterialIndex = iNewIndex;
                m_Material = BDSZ_UIParticlesResMgr.Instance.CreateMaterial(m_ShaderType, m_TextureRes.MainTexture);
                BDSZ_UIParticlesResMgr.Instance.ApplayShaderType(m_Material, m_ShaderType);

                Renderer rd = GetComponent<Renderer>();
                if (rd != null)
                {
                    rd.sharedMaterial = m_Material;
                }
            }
        }
        public override void Render2D(BDSZ_ParticleRoot uc)
        {
            BDSZ_CanvasDraw rd = uc.FindCanvasRender(m_Material, 0);
            RenderParticle(rd, uc, m_Particle);
#if UNITY_EDITOR
            if (BDSZ_UIParticlesResMgr.IsSelected(gameObject))
            {
                DrawSelectBound();
            }
#endif
        }
        protected static readonly Vector2[] s_TextVert = new Vector2[4];
        protected static readonly Vector2[] s_TextUV = new Vector2[4];

        protected void RenderParticle(BDSZ_CanvasDraw rd, BDSZ_ParticleRoot uc, StUIParticleDef p)
        {


            Vector2 vPosition =   new Vector2(p.vCurrentPosition.x + m_vPosition.x, p.vCurrentPosition.y + m_vPosition.y);
            Rect rcArea = uc.GetPixelAdjustedRect();

           
            Vector2 vAreaScale = new Vector2(rcArea.width, rcArea.height);
            Vector2 vXAxis, vYAxis;
            bool bRotate = IsUpdateAttribute(EEffectUpdateAttribute.UpdateAngle) || m_StartAngle.IsZero() == false;

            if (IsEffectAttribute(EEffectAttribute.Billboard))
            {
                
                vXAxis = new Vector2(0.0f, vAreaScale.x);
                vYAxis = new Vector2(vAreaScale.x, 0.0f);
            }
            else if (bRotate == true)
            {
                float fu = Mathf.Sin(p.fCurrentAngle * Mathf.Deg2Rad) * vAreaScale.x;
                float fv = Mathf.Cos(p.fCurrentAngle * Mathf.Deg2Rad) * vAreaScale.y;
                vXAxis = p.vXAxis * fu + p.vYAxis * fv;
                vYAxis = p.vXAxis * fv - p.vYAxis * fu;
            }
            else
            {
                vXAxis =  p.vXAxis * vAreaScale.y;
                vYAxis =  p.vYAxis * vAreaScale.y;
            }

            //vXAxis *= p.fOriginSize;
            Rect rcUV = m_TextureRes.GetUVRect(p.iTexCoordIndex);
            s_TextUV[0] = new Vector2(rcUV.xMin, rcUV.yMin);
            s_TextUV[1] = new Vector2(rcUV.xMin, rcUV.yMax);
            s_TextUV[2] = new Vector2(rcUV.xMax, rcUV.yMax);
            s_TextUV[3] = new Vector2(rcUV.xMax, rcUV.yMin);

            Vector2 vTsPos = new Vector2(vPosition.x * vAreaScale.x, vPosition.y * vAreaScale.y) + new Vector2(rcArea.xMin, rcArea.yMin);

            Vector2 vBottom = vTsPos - p.fHeight * vYAxis * p.fCenterY;
            Vector2 vTop = vTsPos + p.fHeight * vYAxis * (1.0f - p.fCenterY);
            Vector2 vLeft = p.fWidth* vXAxis * p.fCenterX;
            Vector2 vRight = p.fWidth* vXAxis * (1.0f - p.fCenterX);

            s_TextVert[0] = vBottom - vLeft;
            s_TextVert[1] = vTop - vLeft;
            s_TextVert[2] = vTop + vRight;
            s_TextVert[3] = vBottom + vRight;
            rd.DrawQuadrangle(s_TextVert, s_TextUV, 0.0f, p.dwColor, 4);
 

        }

        //粒子属性=============================//

#if UNITY_EDITOR

        public virtual void DrawSelectBound()
        {
            if (m_Particle == null)
                return;
            BDSZ_ParticleRoot pr = m_Controller.GetComponentInParent<BDSZ_ParticleRoot>();
            Rect rcArea = pr.GetPixelAdjustedRect();
            Vector2 vAreaScale = new Vector2(rcArea.width*m_Particle.fWidth, rcArea.height*m_Particle.fHeight);
            Vector2 vPosition = new Vector2(m_vPosition.x*rcArea.width,m_vPosition.y*rcArea.height) + new Vector2(rcArea.xMin, rcArea.yMin);
            m_Controller.DrawSelectedBounds(new Rect(vPosition-vAreaScale*0.5f, vAreaScale));
        }

      
        public EffectFloatMinMax GetStartSize() { return m_StartSize; }
        public void SetStartSize(EffectFloatMinMax value) { m_StartSize = value; }
 
        public EffectFloatMinMax GetCenterX() { return m_fCenterX; }
        public void SetCenterX(EffectFloatMinMax value) { m_fCenterX = value; }
        public EffectFloatMinMax GetCenterY() { return m_fCenterY; }
        public void SetCenterY(EffectFloatMinMax value) { m_fCenterY = value; }

        public EffectFloatMinMax GetStartVelocity() { return m_StartVelocity; }
        public void SetStartVelocity(EffectFloatMinMax value) { m_StartVelocity = value; }
        public EffectFloatMinMax GetStartAngle() { return m_StartAngle; }
        public void SetStartAngle(EffectFloatMinMax value) { m_StartAngle = value; }

        public EffectFloatGradient GetUpdateXScale() { return m_UpdateXScale; }
        public void SetUpdateXScale(EffectFloatGradient value) { m_UpdateXScale = value; }

        public EffectFloatGradient GetUpdateYScale() { return m_UpdateYScale; }
        public void SetUpdateYScale(EffectFloatGradient value) { m_UpdateYScale = value; }


        public EffectFloatGradient GetUpdateVelocity() { return m_UpdateVelocity; }
        public void SetUpdateVelocity(EffectFloatGradient value) { m_UpdateVelocity = value; }

        public EffectFloatGradient GetUpdateAngle() { return m_UpdateAngle; }
        public void SetUpdateAngle(EffectFloatGradient value) { m_UpdateAngle = value; }

        public override void OnValidate()
        {
            if(m_bEmitter==false)
            {
                if(m_Particle!=null)
                {
                    InitParticle(m_Particle, 0.0f);
                }
            }
            if (transform.parent != null)
            {
                BDSZ_UIEffectController parent = transform.parent.gameObject.GetComponent<BDSZ_UIEffectController>();
                if (parent != null)
                {
                    parent.OnValidate();
                }
            }
            base.OnValidate();
        }
        public virtual void OnEnable()
        {
            if (transform.parent != null)
            {
                BDSZ_UIEffectController parent = transform.parent.gameObject.GetComponent<BDSZ_UIEffectController>();
                if (parent != null)
                {
                    parent.AddEffect(this);
                }
            }

        }

#endif

    }
}
