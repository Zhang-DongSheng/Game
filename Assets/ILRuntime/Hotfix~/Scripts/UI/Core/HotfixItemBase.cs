using UnityEngine;

namespace Hotfix.Game.UI
{
    public abstract class HotfixItemBase
    {
        public Transform transform;

        public GameObject entity;

        public string name;

        public bool active;

        public HotfixItemBase(Transform target)
        {
            transform = target;

            entity = target.gameObject;

            name = target.name;

            active = entity.activeSelf;
        }

        public void SetActive(bool active)
        {
            this.active = active;

            if (entity != null && entity.activeSelf != active)
            {
                entity.SetActive(active);
            }
        }
    }
}