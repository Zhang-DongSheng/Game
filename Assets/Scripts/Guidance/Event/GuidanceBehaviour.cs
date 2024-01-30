using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Guidance
{
    public class GuidanceBehaviour : GuidanceBase
    {
        public BehaviourType behaviour;
    }

    public enum BehaviourType
    { 
        None,
        Drag,
        Spring,
    }
}
