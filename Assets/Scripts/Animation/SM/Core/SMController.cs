using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.SM
{
    public class SMController : MonoBehaviour
    {
        [SerializeField] private bool enable;

        [SerializeField] private List<SMBase> sms = new List<SMBase>();

        public UnityEvent onCompleted;

        private SMBase current;

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
            for (int i = 0; i < sms.Count; i++)
            {
                if (sms[i] != null)
                {
                    sms[i].Enable = false;
                }
            }
        }

        private void Next()
        {
            index++;

            if (sms.Count > index)
            {
                for (int i = 0; i < sms.Count; i++)
                {
                    if (i == index)
                    {
                        current = sms[i];
                        break;
                    }
                    else
                    {
                        sms[i].onCompleted.RemoveAllListeners();
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