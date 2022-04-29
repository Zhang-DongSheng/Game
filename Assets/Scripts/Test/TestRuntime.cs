using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRuntime : RuntimeBase
{
    internal override void OnUpdate(float delta)
    {
        Debug.Log(delta);
    }

    ~TestRuntime()
    {
        UnityEngine.Debug.LogError("Îö¹¹º¯Êý2");
    }
}
