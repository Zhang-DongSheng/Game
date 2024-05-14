using UnityEngine;

namespace Game.UI
{
    public class GuidanceLogic : Logic<GuidanceLogic>
    {
        protected override void OnRegister()
        {
            EventManager.Register(EventKey.Guidance, OnTriggerGuidance);
        }

        protected override void OnUnregister()
        {
            EventManager.Unregister(EventKey.Guidance, OnTriggerGuidance);
        }

        private void OnTriggerGuidance(EventMessageArgs args)
        {
            var information = args.Get<GuidanceInformation>(GuidanceConfig.Key);

            UIQuickEntry.Open(UIPanel.Guidance, new UIParameter()
            {
                [GuidanceConfig.Key] = information
            });
        }

        public void Close()
        {
            UIManager.Instance.Close((int)UIPanel.Guidance);
        }
    }
}