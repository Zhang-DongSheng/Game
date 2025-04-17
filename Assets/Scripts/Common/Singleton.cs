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
                    _instance = new GameObject($"[{typeof(T).Name}]").AddComponent<T>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public virtual void Initialize()
    {

    }

    public virtual void Release()
    {
        if (_instance != null)
        {
            GameObject.Destroy(_instance.gameObject);
        }
        _instance = null;
    }
}