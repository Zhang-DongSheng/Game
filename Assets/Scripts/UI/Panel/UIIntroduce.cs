using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIIntroduce : UIBase
    {
        [SerializeField] private ImageBind imgIcon;

        [SerializeField] private ImageBind imgQuality;

        [SerializeField] private TextBind txtName;

        [SerializeField] private TextBind txtType;

        [SerializeField] private TextBind txtDescription;

        [SerializeField] private Button btnClose;

        private void Awake()
        {
            btnClose.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(UIParameter paramter)
        {
            var prop = paramter.Get<PropInformation>("prop");

            if (prop != null)
            {
                RefreshProp(prop);
            }
        }

        private void RefreshProp(PropInformation prop)
        {
            imgIcon.SetSprite(prop.icon);

            imgQuality.SetSprite(string.Format("quality_{0}", (int)prop.quality));

            txtName.SetText(prop.name);

            txtType.SetText(prop.category);

            txtDescription.SetText(prop.description);
        }
    }
}