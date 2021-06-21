using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIReward : UIBase
    {
        [SerializeField] private ParentAndPrefab prefab;

        [SerializeField] private Text title;

        [SerializeField] private Button button;

        private Coroutine coroutine;

        private readonly WaitForSeconds wait = new WaitForSeconds(0.1f);

        private readonly List<ItemProp> items = new List<ItemProp>();

        private void Awake()
        {
            button.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(Paramter paramter)
        {
            title.text = paramter.Get<string>("title");

            coroutine = StartCoroutine(Refresh(paramter.Get<List<PropInformation>>("props")));
        }

        IEnumerator Refresh(List<PropInformation> props)
        {
            if (props != null && props.Count > 0)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    if (i >= items.Count)
                    {
                        items.Add(prefab.Create<ItemProp>());
                    }
                    items[i].Refresh(props[i]);

                    yield return wait;
                }
            }
            yield return null;
        }

        private void OnClickClose()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = null;

            UIManager.Instance.Close(UIPanel.UIReward);
        }
    }
}