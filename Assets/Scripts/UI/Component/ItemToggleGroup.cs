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

        public void Initialize(int count)
        {
            toggles.Clear();

            toggles.AddRange(GetComponentsInChildren<ItemToggle>());

            for (int i = 0; i < count; i++)
            {
                if (i >= toggles.Count)
                {
                    var toggle = prefab.Create<ItemToggle>();
                    toggle.callback = OnClick;
                    toggles.Add(toggle);
                }
                toggles[i].Refresh(i);
            }
            for (int i = count; i < toggles.Count; i++)
            {
                toggles[i].SetActive(false);
            }
        }

        public void Refresh()
        {
            toggles.Clear();

            toggles.AddRange(GetComponentsInChildren<ItemToggle>());

            count = toggles.Count;

            for (int i = 0; i < count; i++)
            {
                toggles[i].callback = OnClick;
                // Refresh Item
                toggles[i].Refresh(i);
            }
        }

        public void Refresh(params int[] parameter)
        {
            toggles.Clear();

            toggles.AddRange(GetComponentsInChildren<ItemToggle>());

            count = toggles.Count;

            for (int i = 0; i < count; i++)
            {
                toggles[i].callback = OnClick;
            }
            count = parameter.Length;

            for (int i = 0; i < count; i++)
            {
                toggles[i].Refresh(parameter[i]);
            }
            for (int i = count; i < toggles.Count; i++)
            {
                toggles[i].SetActive(false);
            }
        }

        public void Select(int index, bool invoke = false)
        {
            count = toggles.Count;

            for (int i = 0; i < count; i++)
            {
                toggles[i].Select(index);
            }
            // Invoke Method
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
}