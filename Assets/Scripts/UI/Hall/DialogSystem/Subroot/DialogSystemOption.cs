using Game.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class DialogSystemOption : ItemBase
    {
        [SerializeField] private List<ItemDialogSystemOption> options;

        public void Refresh(DialogOptionInformation info)
        {

        }
    }
}