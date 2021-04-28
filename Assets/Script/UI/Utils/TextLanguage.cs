using Data;
using Game;

namespace UnityEngine.UI
{
    [ExecuteInEditMode, RequireComponent(typeof(Text))]
    public class TextLanguage : MonoBehaviour
    {
        [SerializeField] private Text component;

        [SerializeField, TextArea] private string text;

        [SerializeField] private bool language;

        private DataLanguage config;

        private string value;

        private void Awake()
        {
            if (component == null)
            {
                component = GetComponent<Text>();
            }
        }

        private void Start()
        {
            config = GameLogic.Instance.Language;
        }

        private void OnValidate()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (language)
            {
                if (config != null)
                {
                    if (config != null && config.Font != null && config.Font != component.font)
                    {
                        component.font = config.Font;
                    }
                    value = config.Word(text);
                }
                component.text = value;
            }
            else
            {
                component.text = text;
            }
        }

        public void SetText(string value)
        {
            text = value; Refresh();
        }
    }
}