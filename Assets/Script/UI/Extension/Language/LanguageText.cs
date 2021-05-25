namespace UnityEngine.UI
{
    /// <summary>
    /// 多语言文本
    /// </summary>
    public class LanguageText : Text
    {
        [SerializeField] private string key;

        [SerializeField] private bool language;

        protected override void Awake()
        {
            base.Awake();

            EventManager.RegisterEvent(EventKey.Language, Refresh);
        }

        protected override void OnDestroy()
        {
            EventManager.UnregisterEvent(EventKey.Language, Refresh);

            base.OnDestroy();
        }

        protected override void OnValidate()
        {
            Refresh(null);

            base.OnValidate();
        }

        private void Refresh(EventMessageArgs args)
        {
            if (language)
            {
                text = LanguageHelper.Instance.Word(key);
            }
        }
    }
}