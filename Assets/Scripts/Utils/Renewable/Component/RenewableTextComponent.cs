using TMPro;
using UnityEngine.UI;

namespace UnityEngine.Renewable
{
    public class RenewableTextComponent : RenewableComponent
    {
        enum RenewableTextType
        {
            Text,
            TexMeshPro,
        }

        [SerializeField] private RenewableTextType type;

        [SerializeField] private Text text;

        [SerializeField] private TextMeshProUGUI textMeshPro;

        public override void Refresh(Object source)
        {
            Refresh(string.Format("{0}", source));
        }

        public void Refresh(string value)
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
            if (text != null)
            {
                text.text = value;
            }
        }

        private void SetTextMeshPro(string value)
        {
            if (textMeshPro != null)
            {
                textMeshPro.text = value;
            }
        }
    }
}