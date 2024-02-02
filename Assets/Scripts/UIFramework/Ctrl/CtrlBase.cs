using Data;
using Game.Resource;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class CtrlBase
    {
        protected UIParameter paramter;

        protected UIBase view;

        private Status status;

        private Action callback;

        public CtrlBase(int panel)
        {
            information = DataUI.Get(panel);

            if (information == null)
            {
                Debuger.LogError(Author.UI, string.Format("{0} Config is Null!", panel));
            }
        }

        public void Open(bool async, Action callback)
        {
            this.callback = callback;

            this.active = true;

            switch (status)
            {
                case Status.None:
                    {
                        if (async)
                            LoadAsync();
                        else
                            Load();
                    }
                    break;
                case Status.Loading:
                    Debuger.LogWarning(Author.UI, string.Format("The panel of [{0}] is loading...", information.name));
                    break;
                case Status.Display:
                    {
                        if (view != null)
                        {
                            view.Reopen(); Show();
                        }
                    }
                    break;
                case Status.Error:
                    Debuger.LogError(Author.UI, string.Format("The panel of [{0}] resource was not found!", information.name));
                    break;
            }
        }

        public void Close(bool destroy = false)
        {
            active = false;

            if (view == null) return;

            if (destroy || information.destroy)
            {
                if (view.gameObject != null)
                {
                    GameObject.Destroy(view.gameObject);
                }
                status = Status.None;
            }
            else
            {
                Hide();
            }
            UIManager.Instance.Record(this);
        }

        public void Paramter(UIParameter paramter)
        {
            this.paramter = paramter;
        }

        public void Display(bool active)
        {
            if (this.active == active) return;

            this.active = active;

            if (view == null) return;

            if (active)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public virtual bool Back()
        {
            if (view != null)
            {
                return view.Back();
            }
            return false;
        }

        private void Load()
        {
            status = Status.Loading;

            try
            {
                Object prefab = ResourceManager.Load(information.path);

                Create(prefab);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.UI, e);
            }
        }

        private void LoadAsync()
        {
            status = Status.Loading;

            try
            {
                ResourceManager.LoadAsync(information.path, (asset) =>
                {
                    Create(asset);
                });
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.UI, e);
            }
        }

        private void Create(Object prefab)
        {
            try
            {
                GameObject entity = GameObject.Instantiate(prefab) as GameObject;

                RectTransform rect = entity.GetComponent<RectTransform>();

                view = entity.GetComponent<UIBase>();

                view.layer = information.layer;

                view.order = information.order;

                view.name = information.name;

                view.transform.SetParent(UIManager.Instance.GetParent(view.layer));

                rect.Reset(); rect.SetFull();

                view.Init(information.panel);

                Show();

                status = Status.Display;
            }
            catch (Exception e)
            {
                Debuger.LogError(Author.UI, e.Message);

                Hide();

                status = Status.Error;
            }
        }

        protected virtual void Show()
        {
            view.Enter();

            view.Refresh(paramter);

            number++;

            callback?.Invoke(); callback = null;

            UIManager.Instance.Sort(view.layer, view.transform);

            UIManager.Instance.Record(this);

            EventManager.Post(EventKey.Open, new EventMessageArgs(information));
        }

        protected virtual void Hide()
        {
            if (view != null)
            {
                view.Exit();
            }
            number--;

            EventManager.Post(EventKey.Close, new EventMessageArgs(information));
        }

        public bool active { get; private set; }

        public int number { get; private set; }

        public UIInformation information { get; private set; }

        enum Status
        {
            None,
            Loading,
            Display,
            Error,
        }
    }
}