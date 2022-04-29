using System.Collections.Generic;
using UnityEngine;

namespace Game.SM
{
    public class SMActive : SMBase
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

        private void SetActive(List<GameObject> items, bool active)
        {
            if (items == null || items.Count == 0) return;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i].activeSelf != active)
                {
                    items[i].SetActive(active);
                }
            }
        }
    }
}