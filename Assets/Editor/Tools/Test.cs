using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static SortUtils;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        [MenuItem("Test/OnClick1")]
        private static void OnClick1()
        {

            List<SortStruct> sorts = new List<SortStruct>();

            sorts.Add(new SortStruct() { ID = 1003 });
            sorts.Add(new SortStruct() { ID = 1001 });
            sorts.Add(new SortStruct() { ID = 1030 });
            sorts.Add(new SortStruct() { ID = 1002 });
            sorts.Add(new SortStruct() { ID = 1900 });
            sorts.Add(new SortStruct() { ID = 1050 });
            sorts.Add(new SortStruct() { ID = 1200 });
            sorts.Add(new SortStruct() { ID = 1300 });
            sorts.Add(new SortStruct() { ID = 1030 });
            sorts.Add(new SortStruct() { ID = 1070 });
            sorts.Add(new SortStruct() { ID = 1001 });

            List<SortStruct> list = SortUtils.QuickSort(sorts);

            for (int i = 0; i < list.Count; i++)
            {
                Debug.LogError(list[i].ID);
            }
        }

        [MenuItem("Test/OnClick2")]
        private static void OnClick2()
        {
            string test = "dd得到张 abcjjjjFFFbbbb  dddd";

            Debug.LogError(test.ToUpperFirst());
        }
    }
}
