using UnityEngine;

namespace Game.UI
{
    public class ItemBase : MonoBehaviour
    {
        private Transform _self;
        public Transform Self
        {
            get
            {
                if (_self == null)
                {
                    _self = transform;
                }
                return _self;
            }
        }

        public virtual void SetPosition(Vector3 position)
        {
            Self.localPosition = position;
        }

        public virtual void SetActive(bool active)
        {
            if (Self != null && Self.gameObject.activeSelf != active)
            {
                Self.gameObject.SetActive(active);
            }
        }
    }
}