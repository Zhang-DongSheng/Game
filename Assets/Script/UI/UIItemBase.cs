using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class UIItemBase : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}
