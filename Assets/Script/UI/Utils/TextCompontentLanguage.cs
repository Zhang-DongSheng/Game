using Data;
using Game;

namespace UnityEngine.UI
{
    [ExecuteInEditMode, RequireComponent(typeof(Text))]
    public class TextCompontentLanguage : MonoBehaviour
    {
        [SerializeField] private Text compontent;

        [SerializeField, TextArea] private string text;

        [SerializeField] private bool language;

        private DataLanguage config;

        private string value;

        private void Awake()
        {
            if (compontent == null)
            {
                compontent = GetComponent<Text>();
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
                    if (config != null && config.Font != null && config.Font != compontent.font)
                    {
                        compontent.font = config.Font;
                    }
                    value = config.Word(text);
                }
                compontent.text = value;
            }
            else
            {
                compontent.text = text;
            }
        }

        public void SetText(string value)
        {
            text = value; Refresh();
        }
    }
}