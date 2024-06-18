using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// Ç©µ½
    /// </summary>
    public class UIActivitySignIn : UIActivityBase
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
            UIQuickEntry.OpenNoticeView("Ç©µ½ID" + signInID);
        }
    }
}