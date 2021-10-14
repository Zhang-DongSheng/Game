using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class BlendData
    {
        public abstract BlendData Lerp(BlendData blend, float progress);
    }
}