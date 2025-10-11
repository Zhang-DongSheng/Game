using UnityEngine;

namespace Game.World
{
    [DisallowMultipleComponent]
    public abstract class EntityBase : RuntimeBehaviour
    {
        protected Vector3 position;

        protected Vector3 rotation;

        protected Vector3 scale = Vector3.one;

        protected bool disposed = false;

        protected GameObject model;

        public virtual void Create(Vector3 position, Vector3 rotation, string name)
        {
            
        }

        public virtual void Dispose()
        {
            disposed = true;
        }
    }
}