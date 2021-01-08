using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private readonly List<Transform> m_parent = new List<Transform>();

        private readonly List<UIBase> m_panel = new List<UIBase>();

        private Canvas canvas;

        private int current_ID;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            Main();
        }

        private void Init()
        {
            canvas = GetComponentInChildren<Canvas>();

            canvas = canvas ?? FindObjectOfType<Canvas>();

            if (canvas != null)
            {
                CanvasScaler scale = canvas.GetComponent<CanvasScaler>();

                if (Screen.width / (float)Screen.height > 16 / 9f)
                {
                    scale.matchWidthOrHeight = 1;
                }
                //生成节点
                for (int i = 0; i < (int)PanelType.Count; i++)
                {
                    PanelType _PT = (PanelType)i;

                    GameObject parent = new GameObject(_PT.ToString());
                    parent.transform.SetParent(canvas.transform);
                    parent.layer = LayerMask.NameToLayer("UI");
                    RectTransform _rect = parent.AddComponent<RectTransform>();

                    _rect.localEulerAngles = Vector3.zero;
                    _rect.localPosition = Vector3.zero;
                    _rect.localScale = Vector3.one;

                    _rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
                    _rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);

                    _rect.anchorMin = Vector2.zero;
                    _rect.anchorMax = Vector2.one;
                    
                    m_parent.Add(_rect);
                }
            }
            else
            {
                Debug.LogError("Can't Find Canvas, Please Add 'Canvas' in Hierarchy!");
            }
        }

        public void Open<T>(string panel_name, bool onlyOne = true, PanelType panel_type = PanelType.Window, PanelEvent panel_event = PanelEvent.None) where T : UIBase, new()
        {
            Transform parent = GetParent(panel_type);

            switch (panel_event)
            {
                case PanelEvent.Hide_Pre:
                    if (m_panel.Count != 0)
                    {
                        m_panel[m_panel.Count - 1].SetActive(false);
                    }
                    break;
                case PanelEvent.Close_Pre:
                    if (m_panel.Count != 0)
                    {
                        m_panel[m_panel.Count - 1].Close();
                        m_panel.RemoveAt(m_panel.Count - 1);
                    }
                    break;
                case PanelEvent.Close_All:
                    for (int i = 0; i < m_panel.Count; i++)
                    {
                        m_panel[i].Close();
                    }
                    m_panel.Clear();
                    break;
                default: break;
            }

            try
            {
                UIBase panel = null;

                if (onlyOne)
                {
                    for (int i = 0; i < m_panel.Count; i++)
                    {
                        if (m_panel[i].panel_name == panel_name)
                        {
                            panel = m_panel[i];
                            m_panel.RemoveAt(i);
                            break;
                        }
                    }

                    if (panel != null)
                    {
                        panel.SetParent(parent);
                        panel.First();
                        panel.Reopen();
                    }
                    else
                    {
                        panel = InitPanel<T>(panel_name, parent);
                    }
                }
                else
                {
                    panel = InitPanel<T>(panel_name, parent);
                }

                panel.panel_type = panel_type;
                panel.panel_name = panel_name;
                panel.panel_ID = NextID();
                panel.Refresh();

                m_panel.Add(panel);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Back()
        {
            if (m_panel.Count != 0)
            {
                m_panel[m_panel.Count - 1].Close();
                m_panel.RemoveAt(m_panel.Count - 1);
            }
            //刷新当前界面
            if (m_panel.Count != 0)
            {
                m_panel[m_panel.Count - 1].Reopen();
                m_panel[m_panel.Count - 1].Refresh();
            }
        }

        public void Close(int panel_ID)
        {
            for (int i = 0; i < m_panel.Count; i++)
            {
                if (m_panel[i].panel_ID == panel_ID)
                {
                    m_panel[i].Close();
                    m_panel.RemoveAt(i);
                    break;
                }
            }
        }

        public void Close(string panel_name)
        {
            for (int i = 0; i < m_panel.Count; i++)
            {
                if (m_panel[i].panel_name == panel_name)
                {
                    m_panel[i].Close();
                    m_panel.RemoveAt(i);
                    break;
                }
            }
        }

        public void Close_All()
        {
            for (int i = 0; i < m_panel.Count; i++)
            {
                m_panel[i].Close();
            }
            m_panel.Clear();
        }

        public void Main()
        {
            for (int i = 0; i < m_panel.Count; i++)
            {
                m_panel[i].Close();
            }
            m_panel.Clear();
        }

        public UIBase Top()
        {
            if (m_panel.Count > 0)
            {
                return m_panel[m_panel.Count - 1];
            }
            else
            {
                return null;
            }
        }

        public UIBase Get(int panel_ID)
        {
            return m_panel.Find(x => x.panel_ID == panel_ID);
        }

        public UIBase Get(string panel_name)
        {
            return m_panel.Find(x => x.panel_name == panel_name);
        }

        public Transform GetParent(PanelType panel_type)
        {
            Transform parent = null;

            try
            {
                parent = m_parent[(int)panel_type];
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }

            return parent;
        }

        private UIBase InitPanel<T>(string panel_name, Transform parent) where T : UIBase
        {
            //panel_name ：资源路径地址，可替换
            string panel_path = panel_name;

            GameObject panel = null;// Factory.Factory.Instance.Pop(panel_name, panel_path, parent);

            UIBase component = panel.GetComponent<T>();
            if (component == null)
            {
                component = panel.AddComponent<T>();
            }

            return component;
        }

        private int NextID()
        {
            if (++current_ID >= int.MaxValue)
            {
                current_ID = 0;
            }
            return current_ID;
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

    public enum PanelType
    {
        Bottom,
        Base,
        Window,
        Widget,
        Top,
        Count,
    }

    public enum PanelEvent
    {
        None,
        Hide_Pre,
        Close_Pre,
        Close_All,
    }
}