using Data;
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

        protected int coin;

        protected override void OnAwake()
        {
            source.onClick.AddListener(OnClickSource);
        }

        public void Refresh()
        {
            Refresh(coin);
        }

        public void Refresh(int coin)
        {
            this.coin = coin;

            var table = DataManager.Instance.Load<DataProp>().Get(coin);

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
            UIQuickEntry.OpenUIBubble(transform, "��Դδ֪��");
        }

        private void OnClickSource()
        {
            UIQuickEntry.OpenUINotice("��Դδ֪��");
        }
    }
}