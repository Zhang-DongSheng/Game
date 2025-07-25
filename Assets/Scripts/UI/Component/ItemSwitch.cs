using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.UI
{
    /// <summary>
    /// ����
    /// </summary>
    public class ItemSwitch : ItemBase, IPointerClickHandler
    {
        [SerializeField] private List<GameObject> off;

        [SerializeField] private List<GameObject> on;

        [SerializeField] private bool active;

        public UnityEvent<bool> onValueChanged;

        public void Initialize(bool active)
        {
            this.active = active;

            Refresh(active);
        }

        private void Refresh(bool active)
        {
            int count = on.Count;

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

            onValueChanged?.Invoke(active);

            Refresh(active);
        }
    }
}