using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 签到
    /// </summary>
    public class SubActivitySignIn : SubviewBase
    {
        [SerializeField] private List<ItemSignIn> items;

        protected override void OnAwake()
        {
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                items[i].callback = OnClickSignIn;
            }
        }

        public override void Refresh()
        {
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                items[i].Refresh(i);
            }
        }

        private void OnClickSignIn(int signInID)
        {
            UIQuickEntry.OpenNoticeView("签到ID" + signInID);
        }
    }
}