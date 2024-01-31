using UnityEngine;

namespace Game.UI
{
    public class GuidanceLogic : Singleton<GuidanceLogic>, ILogic
    {
        public void Initialize()
        {
            EventManager.Register(EventKey.Guidance, OnTriggerGuidance);
        }

        public void Release()
        {
            EventManager.Unregister(EventKey.Guidance, OnTriggerGuidance);
        }

        private void OnTriggerGuidance(EventMessageArgs args)
        {
            var information = args.Get<GuidanceInformation>(GuidanceConfig.Key);

            UIQuickEntry.Open(UIPanel.UIGuidance, new UIParameter()
            {
                [GuidanceConfig.Key] = information
            });
        }

        public void Close()
        {
            UIManager.Instance.Close((int)UIPanel.UIGuidance);
        }
    }
}