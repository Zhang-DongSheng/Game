using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// ¿ØÖÆÌ¨
    /// </summary>
    public class ConsoleView : ViewBase
    {
        [SerializeField] private List<ConsoleBase> views;

        [SerializeField] private RectTransform content;

        [SerializeField] private ItemConsoleDrag drag;

        [SerializeField] private PrefabTemplate prefab;

        private readonly List<ItemConsoleToggle> toggles = new List<ItemConsoleToggle>();

        private int count;

        private void Awake()
        {
            drag.onDrag.AddListener(OnDragValueChanged);

            count = views.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= toggles.Count)
                {
                    var item = prefab.Create<ItemConsoleToggle>();

                    item.Refresh(new ToggleParameter()
                    {
                        index = i,
                        name = views[i].Name,
                        callback = OnClickToggle,
                    });
                    toggles.Add(item);
                }
                views[i].Initialize();
            }
            OnClickToggle(0);
        }

        private void Update()
        {
            count = views.Count;

            for (int i = 0; i < count; i++)
            {
                views[i].Refresh(Time.deltaTime);
            }
        }

        private void OnDragValueChanged(Vector2 delta)
        {
            var view = GetComponent<RectTransform>();

            var height = view.rect.height - delta.y;

            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        private void OnClickToggle(int index)
        {
            count = toggles.Count;

            for (int i = 0; i < count; i++)
            {
                toggles[i].Select(index);
            }
            count = views.Count;

            for (int i = 0; i < count; i++)
            {
                views[i].SetActive(i == index);
            }
        }

        private void OnDestroy()
        {
            count = views.Count;

            for (int i = 0; i < count; i++)
            {
                views[i].Dispose();
            }
        }
    }
}