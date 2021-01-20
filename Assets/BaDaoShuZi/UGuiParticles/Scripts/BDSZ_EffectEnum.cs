using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDSZ_2020
{
    public enum ETextureFlip
    {
        Nothing,
        Horizontally,
        Vertically,
        Both,
    }

    public enum EEffectValueMinMax
    {
        Constant = 0,//固定值//
        RandTwo,//2个值之间随机//
      
    }
    public enum EEffectValueGradient
    {
        Constant = 0,//固定值//
        GradientTwo,//2个值之间渐变//
        GradientThree,//在三个值之间渐变//
        GradientFour,//在多个值之间渐变//
    }

     
    //粒子发射器种类//
    public enum EEmitterType
    {
        Cone = 0,//方向发射器//
        Circle = 1,//球发射器//
        Rect = 2,//立方体发射器//
    }
    
    public enum EEffectAttribute
    {
        Billboard=2<<0,
        Looping=2<<1,
        StretchedBillboard =2<<2,
        FollowParentRotate = 2<<3,
        FollowParentMove=2<<4,

       
    }
    public enum EEffectUpdateAttribute
    {
        UpdateColor = 2<<0,
        UpdateAlpha = 2<<1,
        UpdateAngle = 2<<2,
        UpdateXScale = 2<<3,
        UpdateYScale = 2 << 4,
        UpdateVelocity = 2 << 5,
  
    }

    public enum EEffectBehaviourState
    {
        None = 0,
        Playing,
        End,
    };
   
    public enum EEffectShaderType
    {
        Transparent,
        Additive,
        SoftAdditive,
        ColorAdditive,
        Count,
    }

}
