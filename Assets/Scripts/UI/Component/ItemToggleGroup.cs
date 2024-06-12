using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemToggleGroup : ItemBase
    {
        public PrefabTemplateBehaviour prefab;

        public List<Pair<int, string>> pairs;

        public Action<int> callback;

        private int count;

        private readonly List<ItemToggle> toggles = new List<ItemToggle>();

        private readonly List<ToggleParameter> parameters = new List<ToggleParameter>();

        public void Refresh(params int[] parameter)
        {
            this.parameters.Clear();

            count = parameter.Length;

            for (int i = 0; i < count; i++)
            {
                var key = pairs.Find(x => x.x == parameter[i]);

                var name = key != null ? key.y : parameter[i].ToString();

                this.parameters.Add(new ToggleParameter()
                {
                    index = parameter[i],
                    name = name,
                    callback = OnClick,
                });
            }
            Refresh();
        }

        public void Refresh<T>() where T : Enum
        {
            this.parameters.Clear();

            foreach (var v in Enum.GetValues(typeof(T)))
            {
                var name = v.ToString();

                this.parameters.Add(new ToggleParameter()
                {
                    index = (int)v,
                    name = name,
                    callback = OnClick
                });
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

        private void Refresh()
        {
            this.toggles.Clear();

            this.toggles.AddRange(GetComponentsInChildren<ItemToggle>(true));

            count = parameters.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= toggles.Count)
                {
                    toggles.Add(prefab.Create<ItemToggle>());
                }
                toggles[i].Refresh(parameters[i]);
            }
            for (int i = count; i < toggles.Count; i++)
            {
                toggles[i].SetActive(false);
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
    public class ToggleParameter
    {
        public int index;

        public string name;

        public Action<int> callback;
    }
}