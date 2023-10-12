using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine
{
    public class CodeButton : Button
    {
        [SerializeField] private KeyCode key;

        [SerializeField] private string button;

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