using System;

namespace Game.UI
{
    public class SubPersonalizationBase : ItemBase
    {
        public Action<uint> callback;

        public virtual void Initialise()
        {
            Refresh();
        }

        public virtual void Refresh()
        {

        }

        public void Switch(PersonalizationType type)
        {
            SetActive(type == Type);
        }

        public virtual PersonalizationType Type { get; }
    }
}