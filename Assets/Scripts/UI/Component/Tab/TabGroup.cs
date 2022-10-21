using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private PrefabAndParent prefab;

        private readonly List<Tab> items = new List<Tab>();

        private readonly List<int> list = new List<int>();

        private int count;

        private Action<int> callback;

        public void Initialize(Action<int> action)
        {
            if (callback != null)
            {
                callback = null;
            }
            callback = action;
        }

        public void Refresh(IList<int> list, int index = -1)
        {
            this.list.Clear();

            count = list != null ? list.Count : 0;

            if (count > 0)
            {
                this.list.AddRange(list);
            }
            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    Tab item = prefab.Create<Tab>();
                    item.Initialize(i, OnClick);
                    items.Add(item);
                }
                items[i].Refresh(list[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
            //Ö¸¶¨Ìø×ª
            if (index > -1 && index < count)
            {
                OnClick(index, list[index]);
            }
        }

        public T Item<T>(int index) where T : Tab
        {
            if (items.Count > index && items[index] is T item)
            {
                return item;
            }
            return default;
        }

        public void Select(int index)
        {
            count = items.Count;

            for (int i = 0; i < count; i++)
            {
                if (items[i] != null)
                {
                    items[i].Select(index);
                }
            }
        }

        public void OnClick(int index)
        {
            if (index > -1 && index < count)
            {
                OnClick(index, list[index]);
            }
            else
            {
                Debuger.LogWarning(Author.UI, "Tab Index is Overflow!");
            }
        }

        private void OnClick(int index, int value)
        {
            count = items.Count;

            for (int i = 0; i < count; i++)
            {
                if (items[i] != null)
                {
                    items[i].Select(index);
                }
            }
            callback?.Invoke(index);
        }
    }
}