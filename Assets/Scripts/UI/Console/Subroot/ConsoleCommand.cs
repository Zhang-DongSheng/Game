using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ConsoleCommand : ConsoleBase
    {
        public override string Name => "命令";

        [SerializeField] private TMP_Dropdown dropdownCommand;

        [SerializeField] private InputField inputItem;

        [SerializeField] private InputField inputNumber;

        [SerializeField] private TMP_Text help;

        [SerializeField] private Button submit;

        private string command, value, parameter;

        private readonly List<string> options = new List<string>();

        public override void Initialize()
        {
            dropdownCommand.ClearOptions();

            foreach (var command in ConsoleConfig.Commands)
            {
                options.Add(command.name);
            }
            dropdownCommand.AddOptions(options);

            dropdownCommand.onValueChanged.AddListener(OnCommandValueChanged);

            inputItem.onValueChanged.AddListener(OnItemValueChanged);

            inputNumber.onValueChanged.AddListener(OnNumberValueChanged);

            submit.onClick.AddListener(OnClickSubmit);
        }

        public override void Refresh()
        {
            
        }

        private void OnCommandValueChanged(int value)
        {
            var command = ConsoleConfig.Commands.Find(x => x.index == value);

            if (command == null) return;

            this.command = command.command;

            help.text = command.description;
        }

        private void OnItemValueChanged(string value)
        {
            this.value = value;
        }

        private void OnNumberValueChanged(string value)
        {
            this.parameter = value;
        }

        private void OnClickSubmit()
        {
            Debuger.LogError(Author.Script, $"{command}-{value}-{parameter}");
        }
    }
}