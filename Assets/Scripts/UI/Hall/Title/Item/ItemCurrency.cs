using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCurrency : ItemBase, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI value;

        [SerializeField] private Button source;

        protected override void OnAwake()
        {
            source.onClick.AddListener(OnClickSource);
        }

        public void Refresh()
        {
            SetActive(source, true);

            SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UIQuickEntry.Open(UIPanel.UIBubble, new UIParameter()
            {
                ["value"] = ""
            });
        }

        private void OnClickSource()
        {
            UIQuickEntry.OpenUINotice("À´Ô´Î´Öª£¡");
        }
    }
}