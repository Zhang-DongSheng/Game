using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class RewardView : ViewBase
    {
        [SerializeField] private PrefabTemplateBehaviour prefab;

        [SerializeField] private Text title;

        [SerializeField] private Button button;

        private Coroutine coroutine;

        private readonly WaitForSeconds wait = new WaitForSeconds(0.1f);

        private readonly List<ItemReward> items = new List<ItemReward>();

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(UIParameter paramter)
        {
            Reward reward = paramter.Get<Reward>("reward");

            title.text = reward.title;

            SetActive(true);

            coroutine = StartCoroutine(Refresh(reward));
        }

        IEnumerator Refresh(Reward reward)
        {
            int index = 0;

            int count = reward.props != null ? reward.props.Count : 0;

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

        protected override void OnClickClose()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = null;

            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                items[i].SetActive(false);
            }
            UIManager.Instance.Back();
        }
    }
}