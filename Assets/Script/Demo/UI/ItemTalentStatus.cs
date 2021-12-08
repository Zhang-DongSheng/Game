using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemTalentStatus : MonoBehaviour
    {
        [SerializeField] private GameObject gray;

        [SerializeField] private GameObject green;

        [SerializeField] private GameObject yellow;

        public void Refresh(TalentStatus status)
        {
            switch (status)
            {
                case TalentStatus.None:
                    {
                        SetActive(gray, true);
                        SetActive(green, false);
                        SetActive(yellow, false);
                    }
                    break;
                case TalentStatus.Preview:
                    {
                        SetActive(gray, false);
                        SetActive(green, true);
                        SetActive(yellow, false);
                    }
                    break;
                case TalentStatus.Light:
                    {
                        SetActive(gray, false);
                        SetActive(green, false);
                        SetActive(yellow, true);
                    }
                    break;
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
