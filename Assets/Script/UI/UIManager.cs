﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class UIManager : MonoSingleton<UIManager>
    {
        private readonly List<Transform> _parents = new List<Transform>();

        private readonly Dictionary<UIPanel, CtrlBase> _panels = new Dictionary<UIPanel, CtrlBase>();

        private readonly List<CtrlBase> _records = new List<CtrlBase>();

        private Canvas canvas;

        private int index;

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

                    _parents.Add(rect);
                }
            }
            else
            {
                Debug.LogError("Can't Find Canvas, Please Add 'Canvas' in Hierarchy!");
            }
        }

        private void Register()
        {
            _panels.Add(UIPanel.UIConfirm, new CtrlBase());
        }

        private void Push(UIPanel panel, CtrlBase ctrl)
        {
            if (panel == UIPanel.UILogin ||
                panel == UIPanel.UITest)
            {
                if (_records.Contains(ctrl))
                {
                    Debug.LogWarningFormat("The {0} page is opened repeatedly!", panel);
                }
                else
                {
                    _records.Add(ctrl);
                }
            }
        }

        private void Remove(UIPanel panel)
        {
            for (int i = 0; i < _records.Count; i++)
            {
                if (_records[i].panel == panel)
                {
                    _records.RemoveAt(i);
                    break;
                }
            }
        }

        public void Open(UIPanel panel, UILayer layer = UILayer.None, bool record = false)
        {
            try
            {
                if (_panels.ContainsKey(panel) == false)
                {
                    _panels.Add(panel, new CtrlBase());
                }
                _panels[panel].Open(panel, layer);

                if (record)
                {
                    Push(panel, _panels[panel]);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Back()
        {
            if (_records.Count > 0)
            {
                index = _records.Count - 1;

                _records[index].Close(false);

                _records.RemoveAt(index);

                index = _records.Count - 1;

                if (index > -1 && !_records[index].active)
                {
                    _records[index].Open();
                }
            }
            else
            {
                Debug.LogWarning("this is last panel!");
            }
        }

        public void Close(UIPanel panel, bool destroy = false)
        {
            if (_panels.ContainsKey(panel))
            {
                _panels[panel].Close(destroy);
            }
            Remove(panel);
        }

        public void CloseAll(bool destroy = false)
        {
            foreach (var panel in _panels.Values)
            {
                panel.Close(destroy);
            }
            _records.Clear();
        }

        public CtrlBase GetCtrl(UIPanel key)
        {
            if (_panels.ContainsKey(key) == false)
            {
                _panels.Add(key, new CtrlBase());
            }
            return _panels[key];
        }

        public Transform GetParent(UILayer layer)
        {
            try
            {
                return _parents[(int)layer - 1];
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
        UILotteryDraw,
        UITest,
        Count,
    }
}