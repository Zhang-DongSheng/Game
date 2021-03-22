using TMPro;
using UnityEngine.UI;

namespace UnityEngine.Renewable.Compontent
{
    public class RenewableTextComponent : MonoBehaviour
    {
        enum RenewableTextType
        {
            Text,
            TexMeshPro,
        }

        [SerializeField] private RenewableTextType type;

        [SerializeField] private Text text;

        [SerializeField] private TextMeshProUGUI textMeshPro;

        public void SetContent(string value)
        {
            switch (type)
            {
                case RenewableTextType.Text:
                    SetText(value);
                    break;
                case RenewableTextType.TexMeshPro:
                    SetTextMeshPro(value);
                    break;
            }
        }

        private void SetText(string value)
        {
            if (text == null)
                text = GetComponentInChildren<Text>();
            if (text != null)
                text.text = value;
        }

        private void SetTextMeshPro(string value)
        {
            if (textMeshPro == null)
                textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshPro != null)
                textMeshPro.text = value;
        }
    }
}