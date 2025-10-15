using Game.Logic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class SubPersonalizationNickname : SubPersonalizationBase
    {
        [SerializeField] private TMP_InputField input;

        [SerializeField] private TextBind txtError;

        private void Awake()
        {
            input.onValueChanged.AddListener(OnValueChanged);

            input.onSubmit.AddListener(OnSubmit);
        }

        public override void Refresh()
        {
            input.text = PlayerLogic.Instance.Cache.name;

            txtError.SetTextImmediately(string.Empty);

            callback?.Invoke(0);
        }

        private void OnValueChanged(string value)
        {

        }

        private void OnSubmit(string value)
        {
            PlayerLogic.Instance.Cache.name = value;

            callback?.Invoke(0);
        }

        public override PersonalizationType Type => PersonalizationType.Nickname;
    }
}