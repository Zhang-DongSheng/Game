using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.SAM
{
    public class SAMController : MonoBehaviour
    {
        [SerializeField] private bool enable;

        [SerializeField] private List<SAMBase> sams = new List<SAMBase>();

        public UnityEvent onCompleted;

        private SAMBase current;

        private int index;

        private void OnEnable()
        {
            if (enable)
            {
                Begin();
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < sams.Count; i++)
            {
                if (sams[i] != null)
                {
                    sams[i].Enable = false;
                }
            }
        }

        private void Next()
        {
            index++;

            if (sams.Count > index)
            {
                for (int i = 0; i < sams.Count; i++)
                {
                    if (i == index)
                    {
                        current = sams[i];
                        break;
                    }
                    else
                    {
                        sams[i].onCompleted.RemoveAllListeners();
                    }
                }
                current.onCompleted.AddListener(Next); current.Begin(true);
            }
            else
            {
                onCompleted?.Invoke();
            }
        }

        public void Begin()
        {
            index = -1; Next();
        }

        public void Break()
        {
            if (current != null)
            {
                current.Stop();
            }
        }
    }
}