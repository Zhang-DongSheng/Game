using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VGBasic.FSM
{
    public class ParameterManager
    {
        //单例
        private static ParameterManager instance;

        public static ParameterManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ParameterManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// int类型参数列表
        /// </summary>
        private Dictionary<string, int> intParameter;

        /// <summary>
        /// bool类型参数列表
        /// </summary>
        private Dictionary<string, bool> boolParameter;

        public ParameterManager()
        {
            intParameter = new Dictionary<string, int>();
            boolParameter = new Dictionary<string, bool>();
        }

        /// <summary>
        /// 添加int参数
        /// </summary>
        /// <param name="paraName">参数名称</param>
        /// <param name="value">参数值</param>
        public void AddInt(string paraName, int value)
        {
            if (!intParameter.ContainsKey(paraName))
            {
                intParameter.Add(paraName, value);
            }
            else
            {
                Debug.LogWarning(string.Format("已包含名称为{0}的int参数,无法添加", paraName));
            }
        }

        /// <summary>
        /// 移除int参数
        /// </summary>
        /// <param name="paraName"></param>
        public void RemoveInt(string paraName)
        {
            if (string.IsNullOrEmpty(paraName))
            {
                Debug.LogWarning("参数名称不能为空");
                return;
            }

            if (intParameter.ContainsKey(paraName))
            {
                intParameter.Remove(paraName);
            }
            else
            {
                Debug.LogWarning(string.Format("没有名称为{0}的int参数,无法移除", paraName));
            }
        }

        /// <summary>
        /// 获取int参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public int? GetInt(string paraName)
        {
            if (intParameter.ContainsKey(paraName))
            {
                return intParameter[paraName];
            }

            Debug.LogWarning(string.Format("没有名称为{0}的int参数,返回null", paraName));
            return null;
        }

        /// <summary>
        /// 设置int参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        public void SetInt(string paraName, int value)
        {
            if (intParameter.ContainsKey(paraName))
            {
                intParameter[paraName] = value;
            }
            else
            {
                Debug.LogWarning(string.Format("没有名称为{0}的int参数,请先添加", paraName));
            }
        }

        /// <summary>
        /// 添加bool参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        public void AddBool(string paraName, bool value)
        {
            if (!boolParameter.ContainsKey(paraName))
            {
                boolParameter.Add(paraName, value);
            }
            else
            {
                Debug.LogWarning(string.Format("已包含名称为{0}的bool参数,无法添加", paraName));
            }
        }

        /// <summary>
        /// 移除bool参数
        /// </summary>
        /// <param name="paraName"></param>
        public void RemoveBool(string paraName)
        {
            if (string.IsNullOrEmpty(paraName))
            {
                Debug.LogWarning("参数名称不能为空");
                return;
            }

            if (boolParameter.ContainsKey(paraName))
            {
                boolParameter.Remove(paraName);
            }
            else
            {
                Debug.LogWarning(string.Format("没有名称为{0}的bool参数,无法移除", paraName));
            }
        }

        /// <summary>
        /// 获取bool参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public bool? GetBool(string paraName)
        {
            if (boolParameter.ContainsKey(paraName))
            {
                return boolParameter[paraName];
            }

            Debug.LogWarning(string.Format("没有名称为{0}的bool参数,返回null", paraName));
            return null;
        }

        /// <summary>
        /// 设置bool参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        public void SetBool(string paraName, bool value)
        {
            if (boolParameter.ContainsKey(paraName))
            {
                boolParameter[paraName] = value;
            }
            else
            {
                Debug.LogWarning(string.Format("没有名称为{0}的bool参数,请先添加", paraName));
            }
        }
    }
}
