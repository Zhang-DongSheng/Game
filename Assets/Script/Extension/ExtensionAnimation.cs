using System;
using UnityEngine;

public static partial class Extension
{
    public static void RegisterBeginEvent(this Animation self, Action action = null, string name = "")
    {
        AnimationClip clip = self.Clip(name);

        AddEvent(clip, ClipPlace.Begin);

        Listener(self.gameObject, action);
    }

    public static void RegisterEvent(this Animation self, float time, Action action = null, string name = "")
    {
        AnimationClip clip = self.Clip(name);

        AddEvent(clip, ClipPlace.Any, time);

        Listener(self.gameObject, action);
    }

    public static void RegisterEndEvent(this Animation self, Action action = null, string name = "")
    {
        AnimationClip clip = self.Clip(name);

        AddEvent(clip, ClipPlace.End);

        Listener(self.gameObject, action);
    }

    private static AnimationClip Clip(this Animation self, string name = "")
    {
        if (string.IsNullOrEmpty(name))
            return self.clip;
        else
            return self.GetClip(name);
    }

    private static void AddEvent(AnimationClip clip, ClipPlace place, float time = 0)
    {
        if (clip == null) return;

        switch (place)
        {
            case ClipPlace.Begin:
                time = 0;
                break;
            case ClipPlace.End:
                time = clip.length;
                break;
        }

        foreach (var cp in clip.events)
        {
            if (cp.time == time)
            {
                return;
            }
        }

        AnimationEvent ae = new AnimationEvent()
        {
            functionName = AnimationListener.FUNCTION,
            time = time,
        };
        clip.AddEvent(ae);
    }

    private static void Listener(GameObject self, Action action)
    {
        if (!self.TryGetComponent(out AnimationListener listener))
        {
            listener = self.gameObject.AddComponent<AnimationListener>();
        }
        listener.AddListener(action);
    }

    enum ClipPlace
    {
        Any,
        Begin,
        End,
    }
}