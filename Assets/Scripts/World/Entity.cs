using UnityEngine;

namespace Game.Model
{
    [DisallowMultipleComponent]
    public abstract class Entity : RuntimeBehaviour
    {
        public bool visible { get; private set; }

        private void OnBecameVisible()
        {
            visible = true;

            OnVisibleChanged(visible);
        }

        private void OnBecameInvisible()
        {
            visible = false;

            OnVisibleChanged(visible);
        }

        protected virtual void OnVisibleChanged(bool visible)
        {

        }
    }
}