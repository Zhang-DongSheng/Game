using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ItemTalentNode : MonoBehaviour
    {
        [SerializeField] private Image icon;

        [SerializeField] private Text label;

        [SerializeField] private ItemTalentStatus status;

        [SerializeField] private Button button;

        private Talent talent;

        public Action<int> callback;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Initialize(Talent talent, Action<int> callback)
        {
            this.talent = talent;

            this.callback = callback;

            label.text = string.Format("{0}", talent.ID);

            this.SetActive(talent.active);
        }

        public void Refresh(TalentStatus status)
        {
            this.status.Refresh(status);
        }

        private void OnClick()
        {
            if (talent != null)
            {
                callback?.Invoke(talent.ID);
            }
        }
    }
}