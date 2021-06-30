using System.Collections.Generic;

namespace Game
{
    public class ActivityLogic : Singleton<ActivityLogic>, ILogic
    {
        private readonly List<PopupPanel> panels = new List<PopupPanel>();

        public void Init()
        {
            NetworkEventManager.Register(NetworkEventKey.Activity, OnReceivedInformation);
        }

        #region Function
        public void Popup()
        {
            foreach (var panel in panels)
            {
                if (Popup(panel))
                {
                    break;
                }
            }
        }

        public bool Popup(PopupPanel panel)
        {
            if (panels.Contains(panel))
            {
                return false;
            }
            else
            {
                panels.Add(panel);

                switch (panel)
                {
                    case PopupPanel.SignIn:
                        {
                            if (true)
                            {
                                return true;
                            }
                        }
                        break;
                    case PopupPanel.MonthCard:
                        {
                            if (true)
                            {
                                return true;
                            }
                        }
                        break;
                    case PopupPanel.DailyTask:
                        {
                            if (true)
                            {
                                return true;
                            }
                        }
                        break;
                    default:
                        break;
                }
                return false;
            }
        }
        #endregion

        #region Request
        public void RequestInformation()
        {

        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {

        }
        #endregion
    }

    public enum PopupPanel
    { 
        SignIn,
        MonthCard,
        DailyTask,
    }
}