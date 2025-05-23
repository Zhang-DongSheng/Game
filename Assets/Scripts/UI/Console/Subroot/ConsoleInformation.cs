using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ConsoleInformation : ConsoleBase
    {
        public override string Name => "系统信息";

        [SerializeField] private PrefabTemplate prefab;

        private readonly List<ItemConsoleLabel> items = new List<ItemConsoleLabel>();

        public override void Refresh()
        {
            int index = 0;

            foreach (var info in ConsoleConfig.Infomation)
            {
                if (index >= items.Count)
                {
                    items.Add(prefab.Create<ItemConsoleLabel>());
                }
                items[index++].Refresh(info.Key, info.Value);
            }
        }
    }
}