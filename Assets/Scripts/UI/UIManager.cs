using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class UIManager : MonoSingleton<UIManager>
    {
        private readonly List<Transform> _parents = new List<Transform>();

        private readonly Dictionary<UIPanel, CtrlBase> _panels = new Dictionary<UIPanel, CtrlBase>();

        private readonly Dictionary<UIType, List<CtrlBase>> _records = new Dictionary<UIType, List<CtrlBase>>();

        private Canvas canvas;

        private void Awake()
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

        public void Open(UIPanel panel, bool async = false)
        {
            try
            {
                if (!_panels.ContainsKey(panel))
                {
                    _panels.Add(panel, new CtrlBase(panel));
                }
                _panels[panel].Open(async);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.UI, e);
            }
        }

        public void Parameter(UIPanel panel, UIParameter paramter)
        {
            if (_panels.ContainsKey(panel))
            {
                _panels[panel].Paramter(paramter);
            }
            else
            {
                _panels.Add(panel, new CtrlBase(panel));
                {
                    _panels[panel].Paramter(paramter);
                }
            }
        }

        public void Close(UIPanel panel, bool destroy = false)
        {
            if (_panels.ContainsKey(panel))
            {
                _panels[panel].Close(destroy);
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
                            if (current != null && current != ctrl)
                            {
                                current.Display(false);
                            }
                            current = ctrl;
                        }
                        break;
                    default:
                        Debuger.Log(Author.UI, string.Format("the panel of {0} is unrecord!", ctrl.information.name));
                        break;
                }
                EventManager.Post(EventKey.OpenPanel, new EventMessageArgs(ctrl.information));
            }
            else
            {
                if (_records[key].Contains(ctrl))
                {
                    _records[key].Remove(ctrl);
                }
                EventManager.Post(EventKey.ClosePanel);
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
                _records[UIType.Panel][index].Open();
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

        public bool TryGetCtrl(UIPanel panel, out CtrlBase ctrl)
        {
            if (_panels.ContainsKey(panel))
            {
                ctrl = _panels[panel]; return true;
            }
            else
            {
                ctrl = null; return false;
            }
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

        public Vector2 Resolution()
        {
            if (canvas != null && canvas.TryGetComponent(out RectTransform content))
            {
                return new Vector2(content.rect.width, content.rect.height);
            }
            return new Vector2(Screen.width, Screen.height);
        }

        public static bool IsPointerOverGameObjectWithTag(params string[] tags)
        {
            if (EventSystem.current != null)
            {
                PointerEventData eventData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition,
                };
                List<RaycastResult> raycastResults = new List<RaycastResult>();

                EventSystem.current.RaycastAll(eventData, raycastResults);

                if (raycastResults.Count > 0)
                {
                    if (tags == null || tags.Length == 0)
                    {
                        return true;
                    }
                    else if (tags.Contains(raycastResults[0].gameObject.tag))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public CtrlBase current { get; private set; }
    }
    /// <summary>
    /// UI层级
    /// </summary>
    public enum UILayer
    {
        Bottom,
        Base,
        Window,
        Widget,
        Overlayer,
        Top,
    }
    /// <summary>
    /// UI类型
    /// </summary>
    public enum UIType
    {
        None,
        Panel,
        Popup,
        Widget,
    }
}