using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemDialogSystemOption : ItemBase
    {
        [SerializeField] private TextBind text;

        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh()
        { 
            
        }

        private void OnClick()
        { 
            
        }
    }
}