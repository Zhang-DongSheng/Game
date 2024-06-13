using System.Collections.Generic;
using UnityEngine;

namespace Game.SM
{
    public class SMActive : SMBase
    {
        [SerializeField] private List<GameObject> activates;

        [SerializeField] private List<GameObject> inactives;

        protected override void Initialize()
        {
            
        }

        protected override void Transition(float progress)
        {
            bool active = progress < 0.5f;

            int count = activates.Count;

            for (int i = 0; i < count; i++)
            {
                SetActive(activates[i], active);
            }
            count = inactives.Count;

            for (int i = 0; i < count; i++)
            {
                SetActive(inactives[i], !active);
            }
        }

        private void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}