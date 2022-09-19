using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VGBasic.FSM
{
    public class Transition : ITransition
    {
        #region 基础成员变量

        private IState _from;
        private IState _to;
        private StateTransitionConditionEventHandle condition;

        #endregion

        public Transition(IState from, IState to, StateTransitionConditionEventHandle con)
        {
            _from = from;
            _to = to;
            condition = con;
        }

        public IState From
        {
            get { return _from; }
            set { _from = value; }
        }
        public IState To
        {
            get { return _to; }
            set { _to = value; }
        }
        public StateTransitionConditionEventHandle Condition
        {
            get { return condition; }
            set { condition = value; }
        }
    }
}
