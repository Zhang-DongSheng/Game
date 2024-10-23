using Game.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class DialogSystemView : ViewBase
    {
        [SerializeField] private SubDialogSystemMenu _menu;
        [Display("╫ги╚")]
        [SerializeField] private SubDialogSystemPlayer _player;
        [Display("нд╠╬©Р")]
        [SerializeField] private SubDialogSystemTextbox _textbox;

        protected override void OnAwake()
        {
            _menu.onClickHide = (active) =>
            {
                _textbox.OnClickHide(active);
            };
        }

        public override void Refresh(UIParameter paramter)
        {
            
        }

        public void Refresh()
        {
            
        }
    }
}