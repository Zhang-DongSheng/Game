using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILRuntime.Game.UI
{
    public class StudyView
    {
        public void Init()
        {
            Debuger.Log("Init ...");
        }

        public void Refresh(UIParameter paramter)
        {
            Debuger.Log("Refresh ...");
        }

        public void Release()
        {
            Debuger.Log("Release ...");
        }
    }
}