using UnityEngine;

namespace Game.UI
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
