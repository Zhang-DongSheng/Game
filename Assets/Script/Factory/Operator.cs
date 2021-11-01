using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine
{
    public abstract class Operator : IDisposable
    {
        protected readonly string identification;

        protected readonly int capacity = -1;

        protected string url;

        protected string secret;

        protected Object asset;

        protected readonly Stack<Object> memory = new Stack<Object>();

        protected readonly Stack<Action<Object>> action = new Stack<Action<Object>>();

        public Operator(Data.ResourceInformation resource)
        {
            identification = resource.key;

            url = resource.url;

            secret = resource.secret;

            capacity = resource.capacity;
        }

        public void Pop(Action<Object> action)
        {
            if (memory.Count > 0)
            {
                action?.Invoke(memory.Pop());
            }
            else
            {
                if (asset != null)
                {
                    action?.Invoke(Create());
                }
                else
                {
                    if (this.action != null)
                    {
                        this.action.Push(action);
                    }
                    var _ = Download();
                }
            }
        }

        public void Push(Object asset)
        {
            if (memory.Count < capacity || capacity == -1)
            {
                memory.Push(asset);
            }
            else
            {
                Destroy(asset);
            }
        }

        protected virtual async Task Download()
        {
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<Object>(url);

            await handle.Task;

            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    {
                        asset = handle.Result as Object;

                        while (action.Count > 0)
                        {
                            Pop(action.Pop());
                        }
                    }
                    break;
                case AsyncOperationStatus.Failed:
                    {
                        //Factory.Instance.
                    }
                    break;
            }
        }

        protected abstract Object Create();

        protected abstract void Destroy(Object asset);

        public virtual void Dispose()
        {

        }
    }
}