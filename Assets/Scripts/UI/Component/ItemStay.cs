using UnityEngine;

namespace Game.UI
{
    public class ItemStay : ItemBase
    {
        [SerializeField] private bool sp;

        [SerializeField] private Vector3 position;

        [SerializeField] private bool sr;

        [SerializeField] private Vector3 rotation;

        [SerializeField] private bool ss;

        [SerializeField] private Vector3 scale;

        private Transform target;

        private void Awake()
        {
            target = transform;
        }

        private void Update()
        {
            if (sp)
            {
                target.position = position;
            }

            if (sr)
            {
                target.rotation = Quaternion.Euler(rotation);
            }

            if (ss)
            {
                target.localScale = scale;
            }
        }
    }
}