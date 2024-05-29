using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Guidance
{
    public class ItemGuidance : ItemBase
    {
        [SerializeField] private uint guidanceID;
        
        private bool trigger = false;

        protected override void OnAwake()
        {
            var button = GetComponentInChildren<Button>();

            button.onClick.AddListener(OnPointerClick);
        }

        protected override void OnRegister()
        {
            EventDispatcher.Register(UIEvent.Guidance, OnGuidanceTrigger);
        }

        protected override void OnUnregister()
        {
            EventDispatcher.Unregister(UIEvent.Guidance, OnGuidanceTrigger);
        }

        private void Refresh(GuidanceInformation guidance)
        {
            transform.LiftUpLayer();
        }

        private void OnGuidanceTrigger(EventArgs args)
        {
            var msg = args.Get<GuidanceInformation>(GuidanceConfig.Key);

            trigger = msg.guidanceID == guidanceID;

            if (trigger)
            {
                Refresh(msg);
            }
        }

        public void OnPointerClick()
        {
            if (!trigger) return;

            trigger = false;

            transform.DownLayer();

            GuidanceLogic.Instance.Close();
        }
    }
}