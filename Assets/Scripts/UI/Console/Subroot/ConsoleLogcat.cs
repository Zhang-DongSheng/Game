using Game.Data;
using Game.Logic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ConsoleLogcat : ConsoleBase
    {
        public override string Name => "输出日志";

        public int number = 100;

        [SerializeField] private List<ItemSwitch> switches;

        [SerializeField] private InputField input;

        [SerializeField] private Button btnSave;

        [SerializeField] private Button btnClear;

        [SerializeField] private PrefabTemplate prefab;

        private string search = string.Empty;

        private LogType type = LogType.Log | LogType.Warning | LogType.Error;

        private readonly List<ItemConsoleLog> items = new List<ItemConsoleLog>();

        private readonly List<ConsoleLogInformation> list = new List<ConsoleLogInformation>();

        private readonly List<ConsoleLogInformation> logs = new List<ConsoleLogInformation>();

        public override void Initialize()
        {
            Application.logMessageReceived += LogMessageReceived;

            int count = switches.Count;

            for (int i = 0; i < count; i++)
            {
                var index = i;

                switches[i].onValueChanged.AddListener((value)=>
                {
                    OnClickSwitch(i, value);
                });
            }
            input.onValueChanged.AddListener(OnValueChanged);

            btnSave.onClick.AddListener(OnClickSave);

            btnClear.onClick.AddListener(OnClickClear);
        }

        public override void Dispose()
        {
            Application.logMessageReceived -= LogMessageReceived;
        }

        public override void Refresh()
        {
            list.Clear();

            int count = logs.Count;

            for (int i = count - 1; i > -1; i--)
            {
                if (logs[i].type == type)
                {
                    if (string.IsNullOrEmpty(search))
                    {
                        list.Add(logs[i]);
                    }
                    else if (logs[i].message.Contains(search))
                    {
                        list.Add(logs[i]);
                    }
                }
            }
            count = Mathf.Clamp(list.Count, 0, ConsoleConfig.LOGMAX);

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                { 
                    items.Add(prefab.Create<ItemConsoleLog>());
                }
                items[i].Refresh(list[i]);
            }
        }

        private void LogMessageReceived(string message, string source, LogType type)
        {
            if (logs.Count > 10000)
            { 
                logs.RemoveAt(0);
            }
            logs.Add(new ConsoleLogInformation(type, message, source));

            Refresh();
        }

        private void OnValueChanged(string value)
        {
            search = value;

            Refresh();
        }

        private void OnClickSwitch(int index, bool value)
        {
            switch (index)
            {
                case 0:
                    type = value ? type | LogType.Log : type & ~LogType.Log;
                    break;
                case 1:
                    type = value ? type | LogType.Warning : type & ~LogType.Warning;
                    break;
                case 2:
                    type = value ? type | LogType.Error : type & ~LogType.Error;
                    break;
            }
            Refresh();
        }

        private void OnClickClear()
        {
            logs.Clear(); Refresh();
        }

        private void OnClickSave()
        {
            ConsoleLogic.Instance.SaveLog(list);
        }
    }
}