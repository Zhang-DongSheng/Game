﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 约瑟夫环
/// </summary>
public class StudyJosephRing : MonoBehaviour
{
    private readonly List<int> people = new List<int>();

    private void Start()
    {
        for (int i = 0; i < 41; i++)
        {
            people.Add(i + 1);
        }

        JosephRing(people, 3);
    }

    private void JosephRing(List<int> list, int circle = 3)
    {
        int index = 0, step = 0;

        while (true)
        {
            if (list.Count < 3)
            {
                break;
            }

            if (++step == circle)
            {
                step = 1;

                Debug.LogError("移除：" + list[index]);

                list.RemoveAt(index);
            }

            if (index >= list.Count)
            {
                index = 0;
            }

            if (++index >= list.Count)
            {
                index = 0;
            }
        }

        Debug.LogError("剩余：" + list[0] + " and " + list[1]);
    }
}