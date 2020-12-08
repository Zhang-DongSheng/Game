using System;
using System.Collections.Generic;

namespace UnityEngine.SAM
{
    public class SAMController : MonoBehaviour
    {
        public Action onCompleted;

        [SerializeField] private List<SAMBase> sams = new List<SAMBase>();

        private SAMBase current;

        private int index;

        private void Start()
        {
            Begin();
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

                Debug.LogError("完成了");
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
                current.Close();
            }
        }
    }
}