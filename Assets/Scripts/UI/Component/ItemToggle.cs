using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    /// <summary>
    /// ÇÐ»»°´Å¥
    /// </summary>
    public class ItemToggle : ItemBase, IPointerClickHandler
    {
        [SerializeField] private List<GameObject> background;

        [SerializeField] private List<GameObject> foreground;

        protected Action<int> callback;

        protected int index, count;

        protected string content;

        protected bool active;

        public virtual void Refresh(ItemToggleKey key)
        {
            this.index = key.index;

            content = key.content;

            callback = key.callback;

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

        protected virtual string Content(int index)
        {
            return content;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            callback?.Invoke(index);
        }
    }
}