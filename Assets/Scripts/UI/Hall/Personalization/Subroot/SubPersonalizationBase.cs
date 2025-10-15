using System;

namespace Game.UI
{
    public class SubPersonalizationBase : ItemBase
    {
        public Action<uint> callback;

        protected uint personalizatalID;

        public virtual void Initialise()
        {
            Refresh();
        }

        public virtual void Refresh()
        {

        }

        public void Switch(PersonalizationType type)
        {
            var active = Type == type;

            if (active)
            {
                callback?.Invoke(personalizatalID);
            }
            SetActive(active);
        }

        public virtual PersonalizationType Type { get; }
    }
}