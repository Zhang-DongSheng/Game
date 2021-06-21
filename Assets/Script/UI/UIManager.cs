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

        private readonly List<Transform> parents = new List<Transform>();

        private readonly Dictionary<UIPanel, CtrlBase> panels = new Dictionary<UIPanel, CtrlBase>();

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

                if (Config.ScreenRatio > Config.ResolutionRatio)
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

                    parents.Add(rect);
                }
            }
            else
            {
                Debug.LogError("Can't Find Canvas, Please Add 'Canvas' in Hierarchy!");
            }
        }

        private void Register()
        {
            panels.Add(UIPanel.UIConfirm, new CtrlBase());
        }

        public void Open(UIPanel key, UILayer layer = UILayer.None)
        {
            try
            {
                if (panels.ContainsKey(key)) { }
                else
                {
                    panels.Add(key, new CtrlBase());
                }
                panels[key].Open(key, layer);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Close(UIPanel key, bool destroy = false)
        {
            if (panels.ContainsKey(key))
            {
                panels[key].Close(destroy);
            }
        }

        public void CloseAll(bool destroy = false)
        {
            foreach (var panel in panels.Values)
            {
                panel.Close(destroy);
            }
        }

        public CtrlBase GetCtrl(UIPanel key)
        {
            if (panels.ContainsKey(key) == false)
            {
                panels.Add(key, new CtrlBase());
            }
            return panels[key];
        }

        public Transform GetParent(UILayer layer)
        {
            try
            {
                return parents[(int)layer - 1];
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

        public bool ScreentPointToUGUIPosition(RectTransform parent, Vector2 point, out Vector2 position)
        {
            if (canvas == null)
            {
                position = Vector2.zero; return false;
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera != null)
            {
                return RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, point, canvas.worldCamera, out position);
            }
            else
            {
                return RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, point, null, out position);
            }
        }

        public Canvas Canvas(UIBase view)
        {
            Transform root = view.transform;

            while (root != null)
            {
                if (root.TryGetComponent(out Canvas canvas))
                {
                    return canvas;
                }
                root = root.parent;
            }
            return null;
        }
    }

    public enum UILayer
    {
        None,
        Bottom,
        Base,
        Window,
        Widget,
        Overlayer,
        Top,
    }

    public enum UIPanel
    {
        None,
        UIMain,
        UILogin,
        UILoading,
        UIWaiting,
        UIConfirm,
        UINotice,
        UIReward,
        UITest,
        Count,
    }
}