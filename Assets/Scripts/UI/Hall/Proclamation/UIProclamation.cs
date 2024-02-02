using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIProclamation : UIBase
    {
        [SerializeField] private Text txtContent;

        [SerializeField] private Button btnConfirm;

        protected override void OnAwake()
        {
            btnConfirm.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(UIParameter paramter)
        {
            
        }
    }
}