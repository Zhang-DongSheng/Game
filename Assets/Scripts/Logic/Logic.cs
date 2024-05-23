using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UI
{
    public abstract class Logic<T> where T : class, new()
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        public virtual void Initialize()
        {
            OnRegister();
        }

        public virtual void Release()
        {
            OnUnregister();
        }

        protected virtual void OnRegister()
        {

        }

        protected virtual void OnUnregister()
        {

        }
    }
}