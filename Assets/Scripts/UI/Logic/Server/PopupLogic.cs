using Game.Data;
using Game.UI;
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

            Add(UIPanel.Proclamation);
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

        enum PopupState
        {
            Idle,
            Showing,
        }
    }
}