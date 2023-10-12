using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ItemToggle : ItemBase, IPointerClickHandler
    {
        public Action<int> callback;

        [SerializeField] private List<GameObject> background;

        [SerializeField] private List<GameObject> foreground;

        private int index, count;

        private bool active;

        public virtual void Refresh(int index)
        {
            this.index = index;

            active = false;

            Select(-1);

            SetActive(true);
        }

        public virtual void Select(int index)
        {
            active = this.index.Equals(index);

            count = foreground.Count;

            for (int i = 0; i < count; i++)
            {
                SetActive(foreground[i], active);
            }
            count = background.Count;

            for (int i = 0; i < count; i++)
            {
                SetActive(background[i], !active);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            callback?.Invoke(index);
        }
    }
}