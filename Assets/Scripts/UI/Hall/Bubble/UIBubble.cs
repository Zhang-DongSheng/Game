using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public class UIBubble : UIBase
    {
        [SerializeField] private RectTransform content;

        public override void Refresh(UIParameter parameter)
        {
            
        }

        private void FixedPosition(Vector3 position)
        {
            //var camera = UIManager.Instance.camera;

            //var screen = RectTransformUtility.WorldToScreenPoint( , position);
        }
    }
}
