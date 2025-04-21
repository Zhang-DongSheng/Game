using Game.UI;
using UnityEngine;

namespace ILRuntime.Game.UI
{
    public class ILStudyView : ILViewBase
    {
        public override void Init(Transform transform)
        {
            Debuger.Log("ILStudyView Init ...");
        }

        public override void Refresh(UIParameter paramter)
        {
            Debuger.Log("ILStudyView Refresh ...");
        }

        public override void Release()
        {
            Debuger.Log("ILStudyView Release ...");
        }
    }
}