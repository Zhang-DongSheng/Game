using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIIntroduce : UIBase
    {
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(Paramter paramter)
        {

        }
    }
}