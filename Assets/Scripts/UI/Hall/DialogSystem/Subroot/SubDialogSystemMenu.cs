using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SubDialogSystemMenu : ItemBase
    {
        internal Action onClickShowOrHide;

        internal Action onClickNext;

        internal Action onClickSkip;

        [SerializeField] private Button btnNext;

        [SerializeField] private Button btnSkip;

        [SerializeField] private Button btnHide;

        [SerializeField] private Button btnBack;

        protected override void OnAwake()
        {
            btnNext.onClick.AddListener(OnClickNext);

            btnSkip.onClick.AddListener(OnClickSkip);

            btnHide.onClick.AddListener(OnClickShowOrHide);

            btnBack.onClick.AddListener(OnClickBack);
        }

        public void RefreshDisplay(bool active)
        {
            var component = btnHide.GetComponentInChildren<TextBind>();

            if (component != null)
            {
                component.SetText(active ? "Show" : "Hide");
            }
        }

        private void OnClickNext()
        {
            onClickNext?.Invoke();
        }

        private void OnClickSkip()
        {
            onClickSkip?.Invoke();
        }

        private void OnClickShowOrHide()
        {
            onClickShowOrHide?.Invoke();
        }

        private void OnClickBack()
        {
            UIManager.Instance.Close((int)UIPanel.DialogSystem);
        }
    }
}