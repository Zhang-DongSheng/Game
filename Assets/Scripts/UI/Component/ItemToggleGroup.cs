using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemToggleGroup : ItemBase
    {
        public PrefabTemplateBehaviour prefab;

        public Action<int> callback;

        private int count;

        private readonly List<ItemToggle> toggles = new List<ItemToggle>();

        private readonly List<ItemToggleKey> keys = new List<ItemToggleKey>();

        private void Refresh()
        {
            this.toggles.Clear();

            this.toggles.AddRange(GetComponentsInChildren<ItemToggle>(true));

            count = keys.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= toggles.Count)
                {
                    toggles.Add(prefab.Create<ItemToggle>());
                }
                toggles[i].Refresh(keys[i]);
            }
            for (int i = count; i < toggles.Count; i++)
            {
                toggles[i].SetActive(false);
            }
        }

        public void Refresh(params int[] parameter)
        {
            this.keys.Clear();

            count = parameter.Length;

            for (int i = 0; i < count; i++)
            {
                this.keys.Add(new ItemToggleKey()
                {
                    index = parameter[i],
                    callback = OnClick,
                });
            }
            Refresh();
        }

        public void Refresh<T>() where T : Enum
        {
            this.keys.Clear();

            foreach (var v in Enum.GetValues(typeof(T)))
            {
                this.keys.Add(new ItemToggleKey()
                {
                    index = (int)v,
                    callback = OnClick
                });
            }
            Refresh();
        }

        public void Refresh(List<ItemToggleKey> keys)
        {
            this.keys.Clear();

            this.keys.AddRange(keys);

            foreach (var key in this.keys)
            {
                key.callback = OnClick;
            }
            Refresh();
        }

        public void Select(int index, bool invoke = false)
        {
            count = toggles.Count;

            for (int i = 0; i < count; i++)
            {
                toggles[i].Select(index);
            }

            if (invoke)
            {
                callback?.Invoke(index);
            }
        }

        private void OnClick(int index)
        {
            count = toggles.Count;

            for (int i = 0; i < count; i++)
            {
                toggles[i].Select(index);
            }
            callback?.Invoke(index);
        }
    }
    [Serializable]
    public class ItemToggleKey
    {
        public int index;

        public Action<int> callback;
    }
}