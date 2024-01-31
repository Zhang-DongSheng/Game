using Game.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMain : UIBase
    {
        [SerializeField] private Button combat;

        [SerializeField] private List<ItemEntry> entries;

        [SerializeField] private List<ItemCurrency> currencies;

        private int guidance = 100;

        protected override void OnAwake()
        {
            combat.onClick.AddListener(OnClickCombat);
        }

        public override void Refresh(UIParameter parameter)
        {
            foreach (var entry in entries)
            {
                entry.Refresh();
            }
            RefreshCurrencies();

            MainLogic.Instance.Display((uint)Random.Range(1, 4));

            EventManager.Post(EventKey.Guidance, new EventMessageArgs()
            {
                [GuidanceConfig.Key] = new GuidanceInformation()
                {
                    guidanceID = guidance++
                }
            });
            Debuger.LogError( Author.UI, "触发主界面窗口引导" + (guidance - 1));
        }

        private void RefreshCurrencies()
        {
            var list = new List<uint>() { 101, 102, 103 };

            int count = Mathf.Clamp(list.Count, 0, currencies.Count);

            for (int i = 0; i < count; i++)
            {
                currencies[i].Refresh(list[i]);
            }
            for (int i = count; i < currencies.Count; i++)
            {
                currencies[i].SetActive(false);
            }
        }

        private void OnClickCombat()
        {
            GameStateController.Instance.EnterState<GameCombatState>();
        }
    }
}