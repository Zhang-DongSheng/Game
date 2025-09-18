using Game.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemConsoleLog : ItemBase
    {
        [SerializeField] private Image icon;

        [SerializeField] private Text message;

        [SerializeField] private Text source;

        public void Refresh(ConsoleLogInformation information)
        {
            //icon.sprite = ConsoleConfig.GetIcon(information.type);

            message.text = $"[{information.time.ToString("hh:mm:ss")}]:{information.message}";

            source.text = information.source;
        }
    }
}