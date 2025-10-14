using Game.Audio;
using Game.Logic;
using Game.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class MainView : ViewBase
    {
        [SerializeField] private SubMainTitle title;

        [SerializeField] private SubMainBanner banner;

        private readonly List<ItemEntry> entries = new List<ItemEntry>();

        protected override void OnAwake()
        {
            entries.Clear();

            entries.AddRange(transform.GetComponentsInChildren<ItemEntry>(true));
        }

        public override void Refresh(UIParameter parameter)
        {
            title.Refresh();

            foreach (var entry in entries)
            {
                entry.Refresh();
            }
            ModelDisplayManager.Instance.SwitchGroup(1);

            ModelDisplayManager.Instance.RefreshModel(new ModelDisplayInformation()
            {
                path = "Package/Prefab/Model/Character/MODEL_MECANIM.prefab"
            });
            AudioManager.Instance.PlayMusic("HITA - 醉红绡");

            PopupLogic.Instance.Trigger();
        }
    }
}