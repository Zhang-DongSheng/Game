using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VGBasic.FSM
{
    /// <summary>
    /// 状态基础委托
    /// </summary>
    /// <param name="s"></param>
    public delegate void StateBaseEventHandle(IState s);
    /// <summary>
    /// 状态无参委托
    /// </summary>
    public delegate void StateEmptyEventHandle();

    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        string StateName { get; }

        /// <summary>
        /// 进入状态事件
        /// </summary>
        StateBaseEventHandle OnStateEnter { set; get; }
        /// <summary>
        /// 离开状态事件
        /// </summary>
        StateBaseEventHandle OnStateExit { set; get; }
        /// <summary>
        /// 状态更新事件
        /// </summary>
        StateEmptyEventHandle OnStateUpdate { set; get; }
        /// <summary>
        /// 所处的状态机
        /// </summary>
        IStateMachine Parent { set; get; }
        /// <summary>
        /// 状态过渡列表
        /// </summary>
        List<ITransition> StateTransitions { get; }

        /// <summary>
        /// 添加过度方法
        /// </summary>
        /// <param name="t"></param>
        void AddTransition(ITransition t);
        /// <summary>
        /// 移除过度方法
        /// </summary>
        /// <param name="t"></param>
        void RemoveTransition(ITransition t);
    }
}
