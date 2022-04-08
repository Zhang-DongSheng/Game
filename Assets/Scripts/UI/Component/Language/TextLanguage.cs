using Game;

namespace UnityEngine.UI
{
    /// <summary>
    /// 多语言文本
    /// </summary>
    public class TextLanguage : Text
    {
        [SerializeField] private string key;

        [SerializeField] private bool language;

        protected override void Awake()
        {
            base.Awake();

            EventManager.Register(EventKey.Language, Refresh);
        }

        protected override void OnDestroy()
        {
            EventManager.Unregister(EventKey.Language, Refresh);

            base.OnDestroy();
        }

        private void Refresh(EventMessageArgs args)
        {
            if (language)
            {
                //TextManager.Instance.SetString()
            }
        }
    }
}