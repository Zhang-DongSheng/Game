using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitleBack : ItemBase
    {
        [SerializeField] private Text title;

        [SerializeField] private Button button;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(UIInformation information)
        {
            title.text = information.panel.ToString();

            SetActive(true);
        }

        private void OnClick()
        {
            UIManager.Instance.Back();
        }
    }
}
