using UnityEngine;

namespace Game.Model
{
    [ExecuteInEditMode]
    public class ModelScale : RuntimeBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private bool stay;

        [SerializeField] private Vector3 scale = Vector3.one;

        private void Awake()
        {
            if (target == null)
                target = transform;
            OnTransformParentChanged();
        }

        private void OnValidate()
        {
            OnTransformParentChanged();
        }

        private void OnTransformParentChanged()
        {
            if (stay)
            {
                Stay(scale);
            }
        }

        public void Stay(Vector3 scale)
        {
            Transform node = target.parent;

            Vector3 world = Vector3.one;

            if (target.TryGetComponent(out RectTransform _))
            {
                while (node != null)
                {
                    if (node.TryGetComponent(out Canvas _))
                    {
                        break;
                    }
                    else
                    {
                        world.x *= node.localScale.x;
                        world.y *= node.localScale.y;
                        world.z *= node.localScale.z;
                    }
                    node = node.parent;
                }
            }
            else
            {
                while (node != null)
                {
                    world.x *= node.localScale.x;
                    world.y *= node.localScale.y;
                    world.z *= node.localScale.z;
                    node = node.parent;
                }
            }
            // X
            if (world.x != 0)
            {
                scale.x *= 1 / world.x;
            }
            // Y
            if (world.y != 0)
            {
                scale.y *= 1 / world.y;
            }
            // Z
            if (world.z != 0)
            {
                scale.z *= 1 / world.z;
            }
            target.localScale = scale;
        }

        public void SetScale(float size)
        {
            this.scale.x = size;

            this.scale.y = size;

            this.scale.z = size;

            if (target == null) return;

            if (target.TryGetComponent(out RectTransform _))
            {
                this.scale.z = 1;
            }
            target.localScale = this.scale;
        }

        public void SetScale(Vector3 scale)
        {
            this.scale = scale;

            if (target == null) return;

            if (target.TryGetComponent(out RectTransform _))
            {
                this.scale.z = 1;
            }
            target.localScale = this.scale;
        }
    }
}