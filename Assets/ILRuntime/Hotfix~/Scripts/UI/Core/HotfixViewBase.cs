using Game.UI;
using UnityEngine;

namespace Hotfix.Game.UI
{
    public abstract class HotfixViewBase
    {
        protected Transform transform;

        public virtual void Init(Transform transform)
        {
            this.transform = transform;

            Debuger.Log("Init ...");
        }

        public virtual void Refresh(UIParameter paramter)
        {
            Debuger.Log("Refresh ...");
        }

        public virtual void Release()
        {
            Debuger.Log("Release ...");
        }
    }
}