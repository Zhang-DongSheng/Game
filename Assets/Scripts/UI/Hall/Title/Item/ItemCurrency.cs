using Game.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCurrency : ItemBase, IPointerClickHandler
    {
        [SerializeField] private ImageBind icon;

        [SerializeField] private Text value;

        [SerializeField] private Button source;

        protected uint coin;

        protected override void OnAwake()
        {
            source.onClick.AddListener(OnClickSource);
        }

        public void Refresh()
        {
            Refresh(coin);
        }

        public void Refresh(uint coin)
        {
            this.coin = coin;

            var table = DataProp.Get(coin);

            if (table == null) return;

            icon.SetSprite(table.icon);

            var hold = WarehouseLogic.Instance.GetPropNumber(coin);

            value.SetText(hold);

            SetActive(source, true);

            SetActive(true);
        }

        public void HiddenSource()
        {
            SetActive(source, false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UIQuickEntry.OpenBubbleView(transform, "来源未知！");
        }

        private void OnClickSource()
        {
            switch (coin)
            {
                case 101:
                    UIQuickEntry.Open(UIPanel.Shop);
                    break;
                default:
                    UIQuickEntry.OpenNoticeView("来源未知！");
                    break;
            }
        }
    }
}