using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VGBasic.FSM
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        IState Current { set; get; }

        /// <summary>
        /// 默认状态
        /// </summary>
        IState Default { set; get; }
        
        /// <summary>
        /// 管理的所有状态
        /// </summary>
        List<IState> AllStates { get; }

        /// <summary>
        /// 添加状态方法
        /// </summary>
        /// <param name="s"></param>
        void AddState(IState s);
        
        /// <summary>
        /// 移出状态方法
        /// </summary>
        /// <param name="s"></param>
        void RemoveState(IState s);
        
        /// <summary>
        /// 查找状态方法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IState FindStateWithName(string name);

        /// <summary>
        /// 状态机启动方法
        /// </summary>
        void MachineStart();

        /// <summary>
        /// 检测当前状态过度方法
        /// </summary>
        void CheckCurrentTransition();
        void CheckAnyStateTransition();

        /// <summary>
        /// 任意状态的过度列表
        /// </summary>
        List<ITransition> AnyStateTransitions { get; }
        
        /// <summary>
        /// 添加任意状态过度方法
        /// </summary>
        /// <param name="t"></param>
        void AddAnyStateTransition(ITransition t);
        
        /// <summary>
        /// 移除任意状态过度方法
        /// </summary>
        /// <param name="t"></param>
        void RemoveAnyStateTransition(ITransition t);
        
        /// <summary>
        /// 切换状态方法
        /// </summary>
        /// <param name="s"></param>
        void SwitchState(ITransition s);
    }
}

