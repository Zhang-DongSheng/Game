using UnityEngine;

namespace Game.UI
{
    public class GuidanceLogic : Logic<GuidanceLogic>
    {
        protected override void OnRegister()
        {
            EventDispatcher.Register(UIEvent.Guidance, OnTriggerGuidance);
        }

        protected override void OnUnregister()
        {
            EventDispatcher.Unregister(UIEvent.Guidance, OnTriggerGuidance);
        }

        private void OnTriggerGuidance(EventArgs args)
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