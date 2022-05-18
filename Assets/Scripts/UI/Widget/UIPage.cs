using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class UIPage : MonoBehaviour
    {
        [SerializeField] private PrefabWithParent prefab;

        private readonly List<GameObject> items = new List<GameObject>();

        private IList list;

        private int step, number;

        private int min, max, start, end;

        public Action<GameObject, object> updateItem;

        public void Init(IList list, int number)
        {
            this.list = list;

            this.number = number;

            step = 0;

            min = 0;

            if (list.Count % number != 0)
                max = Mathf.FloorToInt(list.Count / number);
            else
                max = (list.Count / number) - 1;

            Refresh(step);
        }

        public virtual void Next(int step)
        {
            this.step += step;

            if (this.step > max)
            {
                this.step = max;
            }
            else if (this.step < min)
            {
                this.step = min;
            }
            Refresh(this.step);
        }

        public virtual void Jump(int index)
        {
            this.step = index;

            Refresh(this.step);
        }

        protected virtual void Refresh(int step)
        {
            if (step < min || step > max) return;

            start = step * number;

            end = step * number + number;

            if (step == max)
            {
                end = Mathf.Min(end, list.Count);
            }
            Refresh();
        }

        protected virtual void Refresh()
        {
            int index = 0;

            for (int i = start; i < end; i++)
            {
                if (index >= items.Count)
                {
                    items.Add(prefab.Create());
                }
                if (items[index] != null)
                {
                    updateItem?.Invoke(items[index], list[i]);
                }
                SetActive(items[index++], true);
            }
            for (int i = index; i < items.Count; i++)
            {
                SetActive(items[i], false);
            }
        }

        protected void SetActive(GameObject item, bool active)
        {
            if (item != null && item.activeSelf != active)
            {
                item.SetActive(active);
            }
        }
    }
}