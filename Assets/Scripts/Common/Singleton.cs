using System;
using UnityEngine;

public abstract class Singleton<T> where T : new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                try
                {
                    _instance = Activator.CreateInstance<T>();
                }
                catch(Exception e)
                {
                    Debuger.LogException(Author.Script, e);
                }
            }
            return _instance;
        }
    }
}

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>(true);

                if (_instance == null)
                {
                    string name = string.Format("[{0}]", typeof(T).Name);

                    var bind = new GameObject(name);

                    _instance = bind.AddComponent<T>();
                }
                _instance.GetComponent<MonoSingleton<T>>().Initialize();
            }
            return _instance;
        }
    }

    protected virtual void Initialize()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void Release()
    {
        GameObject.Destroy(_instance.gameObject);
    }

    public void Dispose()
    {
        if (_instance != null)
        {
            Release();
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}