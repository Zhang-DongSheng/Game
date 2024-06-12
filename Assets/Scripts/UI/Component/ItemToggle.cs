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

        protected ToggleParameter parameter;

        protected int index, count;

        protected bool active;

        public virtual void Refresh(ToggleParameter parameter)
        {
            this.parameter = parameter;

            index = parameter.index;

            active = false;

            SetContent(index);

            Select(-1);

            SetActive(true);
        }

        public virtual void Select(int index)
        {
            active = this.index == index;

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

        protected virtual void SetContent(int index)
        {
            var components = GetComponentsInChildren<TextBind>(true);

            foreach (var component in components)
            {
                component.SetText(parameter.name);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (parameter != null)
            {
                parameter.callback?.Invoke(index);
            }
        }
    }
}