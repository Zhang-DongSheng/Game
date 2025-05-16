using Game.UI;
using UnityEngine;

namespace Hotfix.Game.UI
{
    public class StudyView : HotfixViewBase
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