using UnityEngine;

namespace Game.UI
{
    public class GuidanceLogic : Singleton<GuidanceLogic>, ILogic
    {
        public void Initialize()
        {

        }

        public void RequestGuidance()
        {
            ScheduleLogic.Instance.Update(Schedule.Guidance);
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