using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VGBasic.FSM
{
    /// <summary>
    /// 状态机
    /// </summary>
    public class StateMachine : State, IStateMachine
    {
        #region 基础成员变量

        IState _current;
        IState _default;
        List<IState> allStates;
        List<ITransition> anyStateTransitions;

        #endregion

        public StateMachine(string name) : base(name)
        {
            allStates = new List<IState>();

            anyStateTransitions = new List<ITransition>();
        }

        public IState Current
        {
            get{ return _current; }
            set{ _current = value; }
        }
        public IState Default
        {
            get{ return _default; }
            set{ _default = value; }
        }
        public List<IState> AllStates
        {
            get{ return allStates; }
        }
        public List<ITransition> AnyStateTransitions
        {
            get{ return anyStateTransitions; }
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="s"></param>
        public void AddState(IState s)
        {
            if (s != null && !allStates.Contains(s))
            {
                //设置默认状态
                if (allStates.Count == 0)
                {
                    _default = s;
                }
                //设置所处的状态机
                s.Parent = this;
                //添加状态
                allStates.Add(s);
            }
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="s"></param>
        public void RemoveState(IState s)
        {
            //当前状态不能移除
            if (_current == s)
            {
                return;
            }
            if (s != null && allStates.Contains(s))
            {
                s.Parent = null;
                allStates.Remove(s);
            }
        }

        /// <summary>
        /// 名字查找状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IState FindStateWithName(string name)
        {
            for (int i = 0; i < allStates.Count; i++)
            {
                if (allStates[i].StateName == name)
                {
                    return allStates[i];
                }
            }
            Debug.LogWarning(string.Format("未找到{0}名称的状态", name));
            
            return null; 
        }

        /// <summary>
        /// 添加任意状态过渡
        /// </summary>
        /// <param name="t"></param>
        public void AddAnyStateTransition(ITransition t)
        {
            if (t != null && !anyStateTransitions.Contains(t))
            {
                anyStateTransitions.Add(t);
            }
        }

        /// <summary>
        /// 移除任意状态过渡
        /// </summary>
        /// <param name="t"></param>
        public void RemoveAnyStateTransition(ITransition t)
        {
            if (t != null && anyStateTransitions.Contains(t))
            {
                anyStateTransitions.Remove(t);
            }
        }

        /// <summary>
        /// 检测当前状态过渡
        /// </summary>
        public void CheckCurrentTransition()
        {
            if (_current == null)
            {
                return;
            }

            //遍历当前状态的所有过度情况
            for (int i = 0; i < _current.StateTransitions.Count; i++)
            {
                //满足过度条件时
                if (_current.StateTransitions[i].Condition())
                {
                    //切换到目标状态
                    SwitchState(_current.StateTransitions[i]);
                    return;
                }
            }
        }

        /// <summary>
        /// 检测任意状态过度
        /// </summary>
        public void CheckAnyStateTransition()
        {
            //遍历任意状态的所有过度情况
            for (int i = 0; i < anyStateTransitions.Count; i++)
            {
                //满足过度条件时
                if (anyStateTransitions[i].Condition())
                {
                    //切换到目标状态
                    SwitchState(anyStateTransitions[i]);
                }
                return;
            }
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="s"></param>
        public void SwitchState(ITransition t)
        {
            //移除上一个状态的更新事件
            if (t.From.OnStateUpdate != null)
            {
                StateUpdate.Instance.updateEvent -= t.From.OnStateUpdate;
            }
            //如果有离开事件
            if (t.From.OnStateExit != null)
            {
                //上一个状态执行离开事件
                t.From.OnStateExit(t.To);
            }
            //更换新的状态
            _current = t.To;
            //如果有下一个状态进入事件
            if (t.To.OnStateEnter != null)
            {
                //执行下一个状态进入事件
                t.To.OnStateEnter(t.From);
            }
            //绑定新状态的更新事件
            if (t.To.OnStateUpdate != null)
            {
                StateUpdate.Instance.updateEvent += t.To.OnStateUpdate;
            }
        }

        /// <summary>
        /// 状态机启动
        /// </summary>
        public void MachineStart()
        {
            //状态机也是一个状态
            if (OnStateEnter != null)
            {
                //执行状态机的进入事件
                OnStateEnter(null);
            }
            if (_current == null)
            {
                _current = Default;
            }

            //状态机必须每帧检测当前状态是否可以切换
            OnStateUpdate += CheckCurrentTransition;

            //状态机必须每帧检测任意状态是否可以切换
            OnStateUpdate += CheckAnyStateTransition;

            //绑定状态机的更新事件
            StateUpdate.Instance.updateEvent += OnStateUpdate;

            if (_current != null)
            {
                if (_current.OnStateEnter != null)
                {
                    //执行当前状态的进入事件
                    _current.OnStateEnter(null);
                }
                if (_current.OnStateUpdate != null)
                {
                    //绑定当前状态的更新事件
                    StateUpdate.Instance.updateEvent += _current.OnStateUpdate;
                }
            }
        }
    }
}
