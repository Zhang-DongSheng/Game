using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine
{
    public class CodeButton : Button
    {
        [SerializeField] private KeyCode key;

        [SerializeField] private string value;

        private void Update()
        {
            if (Input.GetKeyDown(key) || (!string.IsNullOrEmpty(value) && Input.GetButtonDown(value)))
            {
                ExecuteEvents.Execute(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }
    }
}