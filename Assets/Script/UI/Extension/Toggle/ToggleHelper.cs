using System.Collections.Generic;
using UnityEngine.SAM;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleHelper : MonoBehaviour
    {
        [SerializeField] private List<SAMBase> animations;

        [SerializeField] private Transform cursorParent;

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            if (animations != null && animations.Count > 0)
            {
                for (int i = 0; i < animations.Count; i++)
                {
                    if (animations[i] != null)
                    {
                        animations[i].Begin(isOn);
                    }
                }
            }
        }

        public Transform Node
        {
            get
            {
                if (cursorParent != null)
                {
                    return cursorParent;
                }
                return transform;
            }
        }
    }
}