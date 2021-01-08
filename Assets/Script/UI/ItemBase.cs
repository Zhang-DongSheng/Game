using UnityEngine;

namespace Game.UI
{
    public class ItemBase : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        protected virtual void Awake()
        {
            if (target == null)
                target = gameObject;
        }

        public void SetActive(bool active)
        {
            if (target != null && target.activeSelf != active)
            {
                target.SetActive(active);
            }
        }
    }
}