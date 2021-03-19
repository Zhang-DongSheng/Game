using System.Collections.Generic;

namespace UnityEngine.SAM
{
    public class SAMActive : SAMBase
    {
        [SerializeField] private List<GameObject> show;

        [SerializeField] private List<GameObject> hide;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                if (forward)
                {
                    Transition(1);
                }
                else
                {
                    Transition(0);
                }
                Completed();
            }
        }

        protected override void Transition(float step)
        {
            bool active = step > 0.5f;

            for (int i = 0; i < show.Count; i++)
            {
                if (show[i] != null && show[i].activeSelf != active)
                {
                    show[i].SetActive(active);
                }
            }

            for (int i = 0; i < hide.Count; i++)
            {
                if (hide[i] != null && hide[i].activeSelf == active)
                {
                    hide[i].SetActive(!active);
                }
            }
        }
    }
}