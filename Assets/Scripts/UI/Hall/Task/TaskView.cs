using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class TaskView : ViewBase
    {
        [SerializeField] private PrefabTemplateBehaviour prefab;

        private readonly List<ItemTask> items = new List<ItemTask>();

        public override void Refresh(UIParameter parameter)
        {
            var list = TaskLogic.Instance.Tasks;

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(prefab.Create<ItemTask>());
                }
                items[i].Refresh(list[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }
    }
}