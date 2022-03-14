using System.Collections.Generic;
using UnityEngine;

namespace Game.SAM
{
    public class SAMActive : SAMBase
    {
        [SerializeField] private List<GameObject> selected;

        [SerializeField] private List<GameObject> unselected;

        protected override void Init() { }

        protected override void Transition(float step)
        {
            bool active = step > 0.5f;

            SetActive(selected, active);

            SetActive(unselected, !active);
        }

        private void SetActive(List<GameObject> list, bool active)
        {
            if (list == null || list.Count == 0) return;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].activeSelf != active)
                {
                    list[i].SetActive(active);
                }
            }
        }
    }
}