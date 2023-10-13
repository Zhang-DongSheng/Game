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
            UIQuickEntry.OpenUIBubble(transform, "来源未知！");
        }

        private void OnClickSource()
        {
            UIQuickEntry.OpenUINotice("来源未知！");
        }
    }
}