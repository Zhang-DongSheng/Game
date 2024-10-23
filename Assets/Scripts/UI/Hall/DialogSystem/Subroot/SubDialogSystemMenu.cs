using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SubDialogSystemMenu : ItemBase
    {
        internal Action<bool> onClickHide;

        [SerializeField] private Button btnNext;

        [SerializeField] private Button btnSkip;

        [SerializeField] private Button btnHide;

        [SerializeField] private Button btnBack;

        private bool showorhide;

        protected override void OnAwake()
        {
            btnNext.onClick.AddListener(OnClickNext);

            btnSkip.onClick.AddListener(OnClickSkip);

            btnHide.onClick.AddListener(OnClickHide);

            btnBack.onClick.AddListener(OnClickBack);
        }

        private void OnClickNext()
        {

        }

        private void OnClickSkip()
        {

        }

        private void OnClickHide()
        {
            showorhide = !showorhide;

            onClickHide?.Invoke(showorhide);
        }

        private void OnClickBack()
        {
            UIManager.Instance.Close((int)UIPanel.DialogSystem);
        }
    }
}
