using Game.Data;
using Game.UI;
using System;
using System.Collections.Generic;

namespace Game.Logic
{
    public class PopupLogic : Singleton<PopupLogic>, ILogic
    {
        private PopupState _state;

        private readonly List<Popup> _popups = new List<Popup>();

        public void Initialize()
        {
            _popups.Clear();

            if (Condition(UIPanel.Proclamation))
            {
                Add(UIPanel.Proclamation);
            }
        }

        public void Add(UIPanel panel, int parameter = 0, int order = 0)
        {
            if (_popups.Exists(x => x.panel == panel)) return;

            _popups.Add(new Popup()
            {
                panel = panel,
                order = order,
                parameter = parameter,
            });
            // 排序，order大的优先级高

            Trigger();
        }

        public void Trigger()
        {
            if (_state == PopupState.Showing) return;

            if (_popups.Count == 0) return;

            if (!UIManager.Instance.OnlyDisplayed(UIPanel.Main)) return;

            var popup = _popups[0];

            switch (popup.panel)
            {
                default:
                    UIQuickEntry.Open(popup.panel);
                    break;
            }
            _popups.RemoveAt(0);

            _state = PopupState.Showing;
        }

        public void Complete()
        {
            _state = PopupState.Idle;

            Trigger();
        }

        public bool Condition(UIPanel panel)
        {
            var key = $"popup_{panel}_today";

            var value = GlobalVariables.Get<int>(key);

            var today = DateTime.UtcNow.DayOfYear;

            if (value == today)
            {
                return false;
            }
            GlobalVariables.Set(key, today);

            return true;
        }

        enum PopupState
        {
            Idle,
            Showing,
        }
    }
}