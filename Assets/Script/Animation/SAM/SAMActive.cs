using System.Collections.Generic;

namespace UnityEngine.SAM
{
    public class SAMActive : SAMBase
    {
        [SerializeField] private List<GameObject> fore;

        [SerializeField] private List<GameObject> back;

        protected override void Transition(float step)
        {
            bool active = step > 0.5f;

            SetActive(fore, active);

            SetActive(back, !active);
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