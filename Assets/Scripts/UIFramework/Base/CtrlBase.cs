using Game.Data;
using Game.Resource;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class CtrlBase
    {
        protected UIParameter paramter;

        protected ViewBase view;

        private Status status;

        private Action callback;

        public CtrlBase(int panel)
        {
            information = DataUI.Get(panel);

            if (information == null)
            {
                Debuger.LogError(Author.UI, $"{panel} Config is null!");
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
                    Debuger.LogWarning(Author.UI, $"The panel of [{information.name}] is loading...");
                    break;
                case Status.Display:
                    {
                        if (view != null)
                        {
                            view.Reopen();

                            Show();
                        }
                    }
                    break;
                case Status.Error:
                    Debuger.LogError(Author.UI, $"The panel of [{information.name}] resource was not found!");
                    break;
            }
        }

        public void Close(bool destroy = false)
        {
            active = false;

            if (view == null) return;

            if (destroy || information.destroy)
            {
                view.Release();

                view = null;

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

        public void ShowOrHide(bool active)
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

            var prefab = ResourceManager.Load<GameObject>(information.path);

            Create(prefab);
        }

        private void LoadAsync()
        {
            status = Status.Loading;

            ResourceManager.LoadAsync(information.path, (asset) =>
            {
                Create(asset);
            });
        }

        private void Create(Object prefab)
        {
            try
            {
                GameObject entity = GameObject.Instantiate(prefab) as GameObject;

                var rect = entity.GetComponent<RectTransform>();

                view = entity.GetComponent<ViewBase>();

                view.layer = information.layer;

                view.order = information.order;

                view.name = information.name;

                view.transform.SetParent(UIManager.Instance.GetParent(view.layer));

                rect.Reset(); rect.SetFull();

                view.Init(information);

                Show();

                status = Status.Display;
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.UI, e);

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

            EventDispatcher.Post(UIEvent.Open, new UnityEngine.EventArgs(information));
        }

        protected virtual void Hide()
        {
            if (view != null)
            {
                view.Exit();
            }
            number--;

            EventDispatcher.Post(UIEvent.Close, new UnityEngine.EventArgs(information));
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