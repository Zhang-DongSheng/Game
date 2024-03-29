﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class UIManager : MonoSingleton<UIManager>
    {
        private readonly List<Transform> _parents = new List<Transform>();

        private readonly Dictionary<int, CtrlBase> _panels = new Dictionary<int, CtrlBase>();

        private readonly Dictionary<UIType, List<CtrlBase>> _records = new Dictionary<UIType, List<CtrlBase>>();

        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();

            if (canvas != null)
            {
                CanvasScaler scale = canvas.GetComponent<CanvasScaler>();

                var resolution = new Vector2(UIDefine.ResolutionWidth, UIDefine.ResolutionHeight);

                scale.referenceResolution = resolution;

                if (UIDefine.ScreenRatio > UIDefine.ResolutionRatio)
                {
                    scale.matchWidthOrHeight = 1;
                }
                foreach (var layer in Enum.GetValues(typeof(UILayer)))
                {
                    GameObject parent = new GameObject(layer.ToString());

                    parent.transform.SetParent(canvas.transform);

                    parent.layer = LayerMask.NameToLayer("UI");

                    RectTransform rect = parent.AddComponent<RectTransform>();

                    rect.Reset(); rect.SetFull();

                    _parents.Add(rect);

                    Canvas _canvas = parent.AddComponent<Canvas>();

                    parent.AddComponent<GraphicRaycaster>();

                    _canvas.pixelPerfect = true;

                    _canvas.overrideSorting = true;

                    _canvas.sortingOrder = (int)layer * 10;
                }
            }
            else
            {
                Debuger.LogError(Author.UI, "Can't Find Canvas, Please Add 'Canvas' in Hierarchy!");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }

        public void Open(int key, bool async = false, Action callback = null)
        {
            try
            {
                if (!_panels.ContainsKey(key))
                {
                    _panels.Add(key, new CtrlBase(key));
                }
                _panels[key].Open(async, callback);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.UI, e);
            }
        }

        public void Parameter(int key, UIParameter paramter)
        {
            if (_panels.ContainsKey(key))
            {
                _panels[key].Paramter(paramter);
            }
            else
            {
                _panels.Add(key, new CtrlBase(key));
                {
                    _panels[key].Paramter(paramter);
                }
            }
        }

        public void Close(int key, bool destroy = false)
        {
            if (_panels.ContainsKey(key))
            {
                _panels[key].Close(destroy);
            }
        }

        public void CloseAll(bool destroy = false)
        {
            foreach (var panel in _panels.Values)
            {
                panel.Close(destroy);
            }
            _records.Clear();
        }

        public void Record(CtrlBase ctrl)
        {
            if (ctrl == null || ctrl.information == null) return;

            var key = ctrl.information.type;

            if (_records.ContainsKey(key) == false)
            {
                _records.Add(key, new List<CtrlBase>());
            }
            if (ctrl.active)
            {
                if (_records[key].Contains(ctrl))
                {
                    Debuger.LogWarning(Author.UI, string.Format("The panel of {0} is opened repeatedly!", ctrl.information.name));
                }
                else
                {
                    _records[key].Add(ctrl);
                }
                switch (ctrl.information.type)
                {
                    case UIType.Panel:
                        {
                            if (Current != null && Current != ctrl)
                            {
                                Current.Display(false);
                            }
                            Current = ctrl;
                        }
                        break;
                    default:
                        Debuger.Log(Author.UI, string.Format("the panel of {0} is unrecord!", ctrl.information.name));
                        break;
                }
            }
            else
            {
                if (_records[key].Contains(ctrl))
                {
                    _records[key].Remove(ctrl);
                }
            }
        }

        public void Back()
        {
            if (Back(UIType.Popup, 0, out _))
            {
                // 弹窗关闭不影响面板
            }
            else if (Back(UIType.Panel, 1, out int index))
            {
                var ctrl = _records[UIType.Panel][index];

                _records[UIType.Panel].RemoveAt(index);

                ctrl.Display(true);
            }
            else
            {
                Debuger.Log(Author.UI, "This is the final panel!");
            }
        }

        public bool Back(UIType key, int last, out int index)
        {
            index = -1;

            if (_records.ContainsKey(key) && _records[key].Count > last)
            {
                index = _records[key].Count - 1;

                return _records[key][index--].Back();
            }
            return false;
        }

        public bool TryGetCtrl(int key, out CtrlBase ctrl)
        {
            if (_panels.TryGetValue(key, out ctrl))
            {
                return true;
            }
            return false;
        }

        public Transform GetParent(UILayer layer)
        {
            try
            {
                return _parents[(int)layer];
            }
            catch (Exception e)
            {
                Debuger.LogError(Author.UI, e.Message + layer);
            }
            return null;
        }

        public void Sort(UILayer layer, Transform panel)
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

        public Canvas Canvas => canvas;

        public CtrlBase Current { get; private set; }
    }

    public enum UILayer
    {
        Bottom,
        Base,
        Window,
        Widget,
        Overlayer,
        Top,
    }

    public enum UIType
    {
        None,
        Panel,
        Popup,
        Widget,
        Notice,
    }
}