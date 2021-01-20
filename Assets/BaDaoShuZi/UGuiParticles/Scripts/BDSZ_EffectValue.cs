using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace BDSZ_2020
{
    [Serializable]
    public struct EffectFloatMinMax
    {
        public float m_MinValue;
        public float m_MaxValue;
        public EEffectValueMinMax m_Mode;
        public EEffectValueMinMax mode { get { return this.m_Mode; } set { this.m_Mode = value; } }
        public float MinValue { get { return this.m_MinValue; } set { this.m_MinValue = value; } }
        public float MaxValue { get { return this.m_MaxValue; } set { this.m_MaxValue = value; } }

        public float Max {  get { return Mathf.Max(m_MinValue, m_MaxValue); } }
        public float Min { get { return Mathf.Min(m_MinValue, m_MaxValue); } }

        public EffectFloatMinMax(float value)
        {
            this.m_Mode = EEffectValueMinMax.Constant;
            this.m_MinValue = value;
            this.m_MaxValue = value;
        }
        public EffectFloatMinMax(float fMin, float fMax, EEffectValueMinMax mode)
        {
            this.m_Mode = mode;
            this.m_MinValue = fMin;
            this.m_MaxValue = fMax;
        }
      
        public bool IsZero()
        {
            if (m_MinValue == 0.0f && m_MaxValue == 0.0f)
                return true;
            return false;
        }
        public float GetValue()
        {
            float result = 0.0f;
            if (this.m_Mode == EEffectValueMinMax.Constant)
            {
                result = this.m_MinValue;
            }
            else
            {
                result = UnityEngine.Random.Range(m_MinValue, m_MaxValue);
            }
            return result;
        }
    }
    [Serializable]
    public struct EffectColorMinMax
    {
        public Color m_MinValue;
        public Color m_MaxValue;
        public EEffectValueMinMax m_Mode;
        public EEffectValueMinMax mode { get { return this.m_Mode; } set { this.m_Mode = value; } }
        public Color MinValue { get { return this.m_MinValue; } set { this.m_MinValue = value; } }
        public Color MaxValue { get { return this.m_MaxValue; } set { this.m_MaxValue = value; } }

        public EffectColorMinMax(Color value)
        {
            this.m_Mode = EEffectValueMinMax.Constant;
            this.m_MinValue = value;
            this.m_MaxValue = value;
        }
        public EffectColorMinMax(Color fMin, Color fMax, EEffectValueMinMax mode)
        {
            this.m_Mode = mode;
            this.m_MinValue = fMin;
            this.m_MaxValue = fMax;
        }
     
        public Color GetValue()
        {

            Color result;
            if (this.m_Mode == EEffectValueMinMax.Constant)
            {
                result = this.m_MinValue;
            }
            else
            {
                result.a = UnityEngine.Random.Range(m_MinValue.a, m_MaxValue.a);
                result.r = UnityEngine.Random.Range(m_MinValue.r, m_MaxValue.r);
                result.g = UnityEngine.Random.Range(m_MinValue.g, m_MaxValue.g);
                result.b = UnityEngine.Random.Range(m_MinValue.b, m_MaxValue.b);
 
            }
            return result;
        }
    }
    [Serializable]
    public struct EffectFloatGradient
    {
        public float m_MinValue;
        public float m_Key1Value;
        public float m_Key2Value;
        public float m_MaxValue;
        public float m_fTime1Key;
        public float m_fTime2Key;
        public EEffectValueGradient m_Mode;
        public EEffectValueGradient mode { get { return this.m_Mode; } set { this.m_Mode = value; } }
        public float MinValue { get { return this.m_MinValue; } set { this.m_MinValue = value; } }
        public float MaxValue { get { return this.m_MaxValue; } set { this.m_MaxValue = value; } }
        public void SetKeyValue(int iIndex, float Value)
        {
            if (iIndex == 0) m_MinValue = Value;
            else if (iIndex == 1) m_Key1Value = Value;
            else if (iIndex == 2) m_Key2Value = Value;
            else m_MaxValue = Value;
        }
        public void SetTimeValue(int iIndex, float Value)
        {
            if (iIndex == 1) m_fTime1Key = Value;
            else m_fTime2Key = Value;
        }
        public float GetKeyValue(int iIndex)
        {
            if (iIndex == 0) return m_MinValue;
            else if (iIndex == 1) return m_Key1Value;
            else if (iIndex == 2) return m_Key2Value;
            else return m_MaxValue;
        }
        public float GetTimeValue(int iIndex)
        {
            if (iIndex == 0) return 0.0f;
            else if (iIndex == 1) return m_fTime1Key;
            else if (iIndex == 2) return m_fTime2Key;
            else return 1.0f;
        }

        public EffectFloatGradient(float value)
        {
            this.m_MinValue = value;
            this.m_MaxValue = value;
            this.m_Mode = EEffectValueGradient.Constant;
            this.m_fTime1Key = 0.33f;
            this.m_fTime2Key = 0.66f;
            this.m_Key1Value = value;
            this.m_Key2Value = value;
        }

        public EffectFloatGradient(float fMin, float fMax, EEffectValueGradient mode)
        {
            this.m_Mode = mode;
            this.m_MinValue = fMin;
            this.m_MaxValue = fMax;
            this.m_fTime1Key = 0.33f;
            this.m_fTime2Key = 0.66f;
            this.m_Key1Value = Mathf.Lerp(fMin, fMax, this.m_fTime1Key);
            this.m_Key2Value = Mathf.Lerp(fMin, fMax, this.m_fTime2Key);

        }
      

        public bool IsConstant()
        {
            if (m_Mode == EEffectValueGradient.Constant)
                return true;
            else if(m_Mode == EEffectValueGradient.GradientTwo)
            {
                if (m_MinValue == m_MaxValue)
                    return true;
            }
            else if (m_Mode == EEffectValueGradient.GradientThree)
            {
                if (m_MinValue == m_MaxValue && m_MinValue == m_Key1Value)
                    return true;
            }
            else
            {
                if (m_MinValue == m_Key1Value && m_MinValue == m_Key2Value && m_MinValue == m_MaxValue)
                    return true;
            }
            return false;
             
        }
        public float Evaluate(float time)
        {
            time = Mathf.Clamp(time, 0f, 1f);
            float result = 0.0f;
            if (this.m_Mode == EEffectValueGradient.Constant)
            {
                result = this.m_MinValue;
            }
            else if (this.m_Mode == EEffectValueGradient.GradientTwo)
            {
                result = Mathf.Lerp(this.m_MinValue, this.m_MaxValue, time);
            }
            else if (this.m_Mode == EEffectValueGradient.GradientThree)
            {
                float fa = time * 2.0f;
                if (fa <= 1.0f)
                    result = Mathf.Lerp(this.m_MinValue, this.m_MaxValue, fa );
                else
                    result = Mathf.Lerp(this.m_MaxValue, this.m_Key1Value, fa-1.0f);
            }
            else
            {
                if (time <= m_fTime1Key)
                    result = Mathf.Lerp(this.m_MinValue, this.m_Key1Value, time / m_fTime1Key);
                else if (time >= m_fTime2Key)
                    result = Mathf.Lerp(this.m_Key2Value, this.m_MaxValue, (time - m_fTime2Key) / (1.0f - m_fTime2Key));
                else
                {
                    float fa = (time - m_fTime1Key) / (m_fTime2Key - m_fTime1Key);
                    result = Mathf.Lerp(this.m_Key1Value, this.m_Key2Value, fa);
                }
            }
            return result;
        }
    }
    [Serializable]
    public struct EffectColorGradient
    {
        public EEffectValueGradient m_Mode;
        public Color m_MinValue;
        public Color m_Key1Value;
        public Color m_Key2Value;
        public Color m_MaxValue;
        public float m_fTime1Key;
        public float m_fTime2Key;
        public EEffectValueGradient mode { get { return this.m_Mode; } set { this.m_Mode = value; } }
        public Color MinValue { get { return this.m_MinValue; } set { this.m_MinValue = value; } }
        public Color MaxValue { get { return this.m_MaxValue; } set { this.m_MaxValue = value; } }

        public void SetKeyValue(int iIndex, Color Value)
        {
            if (iIndex == 0) m_MinValue = Value;
            else if (iIndex == 1) m_Key1Value = Value;
            else if (iIndex == 2) m_Key2Value = Value;
            else m_MaxValue = Value;
        }
        public void SetTimeValue(int iIndex, float Value)
        {
            if (iIndex == 1) m_fTime1Key = Value;
            else m_fTime2Key = Value;
        }
        public Color GetKeyValue(int iIndex)
        {
            if (iIndex == 0) return m_MinValue;
            else if (iIndex == 1) return m_Key1Value;
            else if (iIndex == 2) return m_Key2Value;
            else return m_MaxValue;
        }
        public float GetTimeValue(int iIndex)
        {
            if (iIndex == 0) return 0.0f;
            else if (iIndex == 1) return m_fTime1Key;
            else if (iIndex == 2) return m_fTime2Key;
            else return 1.0f;
        }

        public EffectColorGradient(Color value)
        {
            this.m_MinValue = value;
            this.m_MaxValue = value;
            this.m_Mode = EEffectValueGradient.Constant;
            this.m_fTime1Key = 0.33f;
            this.m_fTime2Key = 0.66f;
            this.m_Key1Value = value;
            this.m_Key2Value = value;
        }
        public EffectColorGradient(Color fMin, Color fMax, EEffectValueGradient mode)
        {
            this.m_Mode = mode;
            this.m_MinValue = fMin;
            this.m_MaxValue = fMax;
            this.m_fTime1Key = 0.33f;
            this.m_fTime2Key = 0.66f;
            this.m_Key1Value = Color.Lerp(fMin, fMax, this.m_fTime1Key);
            this.m_Key2Value = Color.Lerp(fMin, fMax, this.m_fTime2Key);
        }
       
        public bool IsConstant()
        {
            if (m_Mode == EEffectValueGradient.Constant)
                return true;
            else if (m_Mode == EEffectValueGradient.GradientTwo)
            {
                if (m_MinValue == m_MaxValue)
                    return true;
            }
            else if(m_Mode == EEffectValueGradient.GradientThree)
            {
                if (m_MinValue == m_Key1Value &&  m_MinValue == m_MaxValue)
                    return true;
            }
            else
            {
                if (m_MinValue == m_Key1Value && m_MinValue == m_Key2Value && m_MinValue == m_MaxValue)
                    return true;
            }
            return false;

        }
        public Color Evaluate(float time)
        {
            time = Mathf.Clamp(time, 0f, 1f);
            Color result;
            if (this.m_Mode == EEffectValueGradient.Constant)
            {
                result = this.m_MinValue;
            }
            else if (this.m_Mode == EEffectValueGradient.GradientTwo)
            {
                result = Color.Lerp(this.m_MinValue, this.m_MaxValue, time);
            }
            else if (this.m_Mode == EEffectValueGradient.GradientThree)
            {
                float fa = time * 2.0f;
                if (fa <= 1.0f)
                    result = Color.Lerp(this.m_MinValue, this.m_MaxValue, fa);
                else
                    result = Color.Lerp(this.m_MaxValue, this.m_Key1Value, fa-1.0f);
            }
            else
            {
                if (time <= m_fTime1Key)
                    result = Color.Lerp(this.m_MinValue, this.m_Key1Value, time / m_fTime1Key);
                else if (time >= m_fTime2Key)
                    result = Color.Lerp(this.m_Key2Value, this.m_MaxValue, (time - m_fTime2Key) / (1.0f - m_fTime2Key));
                else
                {
                    float fa = (time - m_fTime1Key) / (m_fTime2Key - m_fTime1Key);
                    result = Color.Lerp(this.m_Key1Value, this.m_Key2Value, fa);
                }
            }
            return result;
        }
    }
    
}