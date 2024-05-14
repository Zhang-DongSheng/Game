using Game.UI.Guidance;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class GuidanceView : ViewBase
    {
        [SerializeField] private ItemGuidanceMask mask;

        [SerializeField] private List<ItemGuidanceBase> items;

        public override void Refresh(UIParameter paramter)
        {
            
        }
    }
}