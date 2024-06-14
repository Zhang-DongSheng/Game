using UnityEngine;

namespace ILRuntime.Game.UI
{
    public abstract class ILItemBase
    {
        public Transform transform;

        public GameObject entity;

        public string name;

        public bool active;

        public ILItemBase(Transform target)
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