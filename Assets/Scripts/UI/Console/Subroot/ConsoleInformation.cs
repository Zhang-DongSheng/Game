using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ConsoleInformation : ConsoleBase
    {
        public override string Name => "系统信息";

        [SerializeField] private PrefabTemplate prefab;

        private readonly List<ItemKeyValue> items = new List<ItemKeyValue>();

        public override void Initialize()
        {
            int count = ConsoleConfig.Infomation.Length;

            for (int i = 0; i < count; i++)
            {
                var values = ConsoleConfig.Infomation[i].Split("|");

                if (i >= items.Count)
                {
                    items.Add(prefab.Create<ItemKeyValue>());
                }
                items[i].Refresh(values[0], values[1]);
            }
        }
    }
}