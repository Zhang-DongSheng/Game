using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ItemSwitch : ItemBase, IPointerClickHandler
    {
        public Action<bool> callback;

        [SerializeField] private List<GameObject> off;

        [SerializeField] private List<GameObject> on;

        private int count;

        private bool active;

        public void Initialize(bool active)
        {
            this.active = active;

            Refresh(active);
        }

        private void Refresh(bool active)
        {
            count = on.Count;

            for (int i = 0; i < count; i++)
            {
                SetActive(on[i], active);
            }
            count = off.Count;

            for (int i = 0; i < count; i++)
            {
                SetActive(off[i], !active);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            active = !active;

            callback?.Invoke(active);

            Refresh(active);
        }
    }
}