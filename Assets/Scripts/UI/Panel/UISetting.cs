using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UISetting : UIBase
    {
        [SerializeField] private List<Toggle> toggles;

        [SerializeField] private List<UISettingBase> views;

        protected override void OnAwake()
        {
            int count = toggles.Count;

            for (int i = 0; i < count; i++)
            {
                int index = i;

                if (toggles[i] != null)
                {
                    toggles[i].onValueChanged.AddListener((isOn) =>
                    {
                        if (isOn)
                        {
                            OnClickTab(index);
                        }
                    });
                }
            }
        }

        protected override void OnUpdate(float delta)
        {
            Debuger.Log(Author.Script, "Set");
        }

        private void OnClickTab(int index)
        {
            int count = views.Count;

            for (int i = 0; i < count; i++)
            {
                SetActive(views[i], i == index);
            }
        }
    }
}