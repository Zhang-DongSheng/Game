using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemDeploy : ItemBase, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Action<int, Vector3> callback;

        [SerializeField] private Text text;

        protected ScrollRect scroll;

        protected int index;

        protected bool drag;

        protected override void OnAwake()
        {
            scroll = GetComponentInParent<ScrollRect>();
        }

        public virtual void Refresh(int index)
        {
            this.index = index;

            text.text = index.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            drag = !eventData.delta.IsVertical();

            if (drag)
            {
                scroll.OnBeginDrag(eventData);
            }
            else
            {
                callback?.Invoke(index, eventData.pointerCurrentRaycast.worldPosition);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (drag)
            {
                scroll.OnDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (drag)
            {
                scroll.OnEndDrag(eventData);
            }
        }
    }
}
