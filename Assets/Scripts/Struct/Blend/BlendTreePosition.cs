using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class BlendTreePosition : BlendTree<BlendPosition>
    {
        
    }

    [System.Serializable]
    public class BlendPosition : BlendBase
    {
        public Vector3 position;

        public override BlendBase Copy(BlendBase target)
        {
            if (target is BlendPosition _target)
            {
                position = _target.position;
            }
            return this;
        }

        public override BlendBase Lerp(BlendBase from, BlendBase to, float progress)
        {
            if (from is BlendPosition _from && to is BlendPosition _to)
            {
                position = Vector3.Lerp(_from.position, _to.position, progress);
            }
            return this;
        }
    }
}