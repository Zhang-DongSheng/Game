using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VGBasic.FSM
{
    /// <summary>
    /// 状态过度条件委托
    /// </summary>
    /// <returns></returns>
    public delegate bool StateTransitionConditionEventHandle();

    /// <summary>
    /// 状态过度接口
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// 从哪个状态来
        /// </summary>
        IState From { set; get; }
        
        /// <summary>
        /// 到哪个状态去
        /// </summary>
        IState To { set; get; }
        
        /// <summary>
        /// 状态过度条件事件
        /// </summary>
        StateTransitionConditionEventHandle Condition { set; get; }
    }
}

