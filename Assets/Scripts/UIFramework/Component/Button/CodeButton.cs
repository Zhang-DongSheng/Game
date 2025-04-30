using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine
{
    public class CodeButton : Button
    {
        [SerializeField] private KeyCode key;

        [SerializeField] private string button;
        /// <summary>
        /// [慎用]模拟按键 按键对用表 http://www.doc88.com/p-895906443391.html
        /// </summary>
        /// <param name="bVk">键值</param>
        /// <param name="bScan">0</param>
        /// <param name="dwFlags">0按下，1按住，2释放</param>
        /// <param name="dwExtraInfo">0</param>
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void Keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private void Update()
        {
            if (key != KeyCode.None)
            {
                if (Input.GetKeyDown(key))
                {
                    ExecuteEvents.Execute(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                }
            }
            else if (!string.IsNullOrEmpty(button))
            {
                if (Input.GetButtonDown(button))
                {
                    ExecuteEvents.Execute(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                }
            }
        }
    }
}