using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ItemClose : ItemBase, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            var view = GetComponentInParent<UIBase>();

            view.Back();
        }
    }
}