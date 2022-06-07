using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// Ã· æ√Ê∞Â
    /// </summary>
    public class UITips : UIBase
    {
        [SerializeField] private Text content;

        [SerializeField] private Button close;

        private void Awake()
        {
            close.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(Paramter paramter)
        {
            content.text = paramter.Get<string>("tips");
        }
    }
}