using UnityEngine;

namespace Game.Develop
{
    public abstract class DevelopBase
    {
        protected Vector2 scroll;

        public abstract void Initialize();

        public abstract void Refresh();

        public virtual void Register()
        {

        }

        public virtual void Unregister()
        {

        }

        public abstract string Name { get; }
    }
}