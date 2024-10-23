using Game.SM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public class SubDialogSystemTextbox : ItemBase
    {
        [SerializeField] private SMTranslate _animation;

        [SerializeField] private TextBind _txtForeground;

        [SerializeField] private TextBind _txtBackground;

        public void OnClickHide(bool active)
        {
            _animation.Begin(active);
        }
    }
}
