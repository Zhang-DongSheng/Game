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
        /// [����]ģ�ⰴ�� �������ñ� http://www.doc88.com/p-895906443391.html
        /// </summary>
        /// <param name="bVk">��ֵ</param>
        /// <param name="bScan">0</param>
        /// <param name="dwFlags">0���£�1��ס��2�ͷ�</param>
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