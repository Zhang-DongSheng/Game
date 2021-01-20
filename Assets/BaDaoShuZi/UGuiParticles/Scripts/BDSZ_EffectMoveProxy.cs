using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDSZ_2020
{
#if UNITY_EDITOR
    public class BDSZ_EffectMoveProxy : BDSZ_EffectBehaviour
#else
    public class BDSZ_EffectMoveProxy : MonoBehaviour
#endif
    {
        public virtual void OnInitParticle(StUIParticleDef p)
        {

        }
        public virtual void UpdateMove(StUIParticleDef p,float fUpdateAlpha,float fDeltaTime)
        {

        }
        public virtual void Reset()
        {

        }
        public virtual void Start()
        {

        }
        public virtual void UpdateEffect(float fLifeTime, float fDeltaTime)
        {

        }
    }
}