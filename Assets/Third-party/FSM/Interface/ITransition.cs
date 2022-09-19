using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

namespace FSM
{
    public interface ITransition
    {
        IState From { get; }

        IState To { get; }

        bool Condition();
    }
}