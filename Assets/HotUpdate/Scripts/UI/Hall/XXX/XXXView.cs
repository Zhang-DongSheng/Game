using UnityEngine;

namespace Hotfix.UI
{
    public class XXXView : HotfixViewBase
    {
        public override void Initialise(Transform target)
        {
            Debug.LogError("XXXView Initialise");
        }

        public override void Refresh()
        {
            Debug.LogError("XXXView Refresh");
        }

        public override void Release()
        {
            Debug.LogError("XXXView Release");
        }
    }
}