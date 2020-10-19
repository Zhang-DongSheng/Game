using Data;

namespace UnityEngine.UI
{
    [ExecuteInEditMode, RequireComponent(typeof(Text))]
    public class TextCompontentLanguage : MonoBehaviour
    {
        [SerializeField] private Text compontent;

        [SerializeField, TextArea] private string text;

        [SerializeField] private bool language;

        private string value;

        DataLanguage data;

        private void Awake()
        {
            if (compontent == null)
                compontent = GetComponent<Text>();

            //Test ...
            data = DataManager.Instance.Load<DataLanguage>("language", "Data/Language");

            data.Init();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Refresh();
        }
#endif

        private void Refresh()
        {
            if (language)
            {
                if (data != null)
                {
                    if (data.Font != null && compontent.font != data.Font)
                    {
                        compontent.font = data.Font;
                    }
                    value = data.Word(text);
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