using Game.Logic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ConsoleCommand : ConsoleBase
    {
        public override string Name => "命令";

        [SerializeField] private Text label;

        [SerializeField] private InputField input;

        [SerializeField] private Button submit;

        [SerializeField] private PrefabTemplate prefab;

        private string content, value;

        private readonly List<ItemConsoleLabel> items = new List<ItemConsoleLabel>();

        public override void Initialize()
        {
            input.onValueChanged.AddListener(OnValueChanged);

            submit.onClick.AddListener(OnClickSubmit);

            OnValueChanged(string.Empty);
        }

        private void OnValueChanged(string value)
        {
            this.value = value.ToLower();

            int index = 0;

            foreach (var cell in ConsoleConfig.Commands)
            {
                if (cell.Key.StartsWith(this.value))
                {
                    if (index >= items.Count)
                    {
                        items.Add(prefab.Create<ItemConsoleLabel>());
                    }
                    items[index++].Refresh(cell.Key, cell.Value);
                }
            }
            for (int i = index; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }

        private void OnClickSubmit()
        {
            content += value + "\n";

            label.text = content;

            ConsoleLogic.Instance.ExecuteCommand(value);
        }
    }
}