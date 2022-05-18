using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// ½±Àø
    /// </summary>
    public class UIReward : UIBase
    {
        [SerializeField] private PrefabWithParent prefab;

        [SerializeField] private Text title;

        [SerializeField] private Button button;

        private Coroutine coroutine;

        private readonly WaitForSeconds wait = new WaitForSeconds(0.1f);

        private readonly List<ItemReward> items = new List<ItemReward>();

        private void Awake()
        {
            button.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(Paramter paramter)
        {
            Reward reward = paramter.Get<Reward>("reward");

            title.text = reward.title;

            SetActive(true);

            coroutine = StartCoroutine(Refresh(reward));
        }

        IEnumerator Refresh(Reward reward)
        {
            int count = reward.currencies != null ? reward.currencies.Count : 0;

            int index = 0;

            for (int i = 0; i < count; i++)
            {
                if (index >= items.Count)
                {
                    items.Add(prefab.Create<ItemReward>());
                }
                items[index++].Refresh(reward.currencies[i]);

                yield return wait;
            }

            count = reward.props != null ? reward.props.Count : 0;

            for (int i = 0; i < count; i++)
            {
                if (index >= items.Count)
                {
                    items.Add(prefab.Create<ItemReward>());
                }
                items[index++].Refresh(reward.props[i]);

                yield return wait;
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

            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
            UIManager.Instance.Close(UIPanel.UIReward);
        }
    }
}