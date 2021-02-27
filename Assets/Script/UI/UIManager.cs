using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Canvas canvas;

        private readonly List<Transform> m_parent = new List<Transform>();

        private readonly Dictionary<UIKey, UICtrl> m_panel = new Dictionary<UIKey, UICtrl>();

        private void Awake()
        {
            Init(); Register();
        }

        private void Init()
        {
            canvas = GetComponentInChildren<Canvas>();

            if (canvas != null)
            {
                CanvasScaler scale = canvas.GetComponent<CanvasScaler>();

                if (UIConfig.ScreenRatio > UIConfig.ResolutionRatio)
                {
                    scale.matchWidthOrHeight = 1;
                }

                foreach (var layer in Enum.GetValues(typeof(UILayer)))
                {
                    if ((UILayer)layer == UILayer.None) continue;

                    GameObject parent = new GameObject(layer.ToString());

                    parent.transform.SetParent(canvas.transform);

                    parent.layer = LayerMask.NameToLayer("UI");

                    RectTransform rect = parent.AddComponent<RectTransform>();

                    rect.Reset(); rect.Full();

                    Canvas _canvas = parent.AddComponent<Canvas>();

                    _canvas.pixelPerfect = true;

                    _canvas.overrideSorting = true;

                    _canvas.sortingOrder = (int)layer * 10;

                    parent.AddComponent<GraphicRaycaster>();

                    m_parent.Add(rect);
                }
            }
            else
            {
                Debug.LogError("Can't Find Canvas, Please Add 'Canvas' in Hierarchy!");
            }
        }

        private void Register()
        {
            m_panel.Add(UIKey.UINotice, new UICtrl());
            m_panel.Add(UIKey.UIConfirm, new UICtrl());
        }

        public void Open(UIKey key, UILayer layer = UILayer.None)
        {
            try
            {
                if (m_panel.ContainsKey(key)) { }
                else
                {
                    m_panel.Add(key, new UICtrl());
                }
                m_panel[key].Open(key, layer);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Close(UIKey key, bool destroy = false)
        {
            if (m_panel.ContainsKey(key))
            {
                m_panel[key].Close(destroy);
            }
        }

        public void CloseAll(bool destroy = false)
        {
            foreach (var panel in m_panel.Values)
            {
                panel.Close(destroy);
            }
        }

        public UICtrl GetCtrl(UIKey key)
        {
            if (m_panel.ContainsKey(key))
            {
                return m_panel[key];
            }
            return null;
        }

        public Transform GetParent(UILayer layer)
        {
            try
            {
                return m_parent[(int)layer - 1];
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return null;
        }

        public void SortDisplay(UILayer layer, Transform panel)
        {
            panel.SetAsLastSibling();

            List<UIBase> childs = GetParent(layer).GetComponentsInChildren<UIBase>(true).ToList();

            childs.Sort((a, b) =>
            {
                if (a.order != b.order)
                {
                    return a.order > b.order ? 1 : -1;
                }
                else
                {
                    return 0;
                }
            });

            for (int i = 0; i < childs.Count; i++)
            {
                childs[i].transform.SetSiblingIndex(i);
            }
        }

        public Vector2 Resolution
        {
            get
            {
                if (canvas != null && canvas.TryGetComponent(out RectTransform content))
                {
                    return new Vector2(content.rect.width, content.rect.height);
                }
                return new Vector2(Screen.width, Screen.height);
            }
        }
    }

    public enum UILayer
    {
        None,
        Bottom,
        Base,
        Window,
        Widget,
        Top,
    }

    public enum UIKey
    {
        None,
        UILogin,
        UILoading,
        UIMain,
        UITest,
        UIConfirm,
        UINotice,
        Count,
    }
}