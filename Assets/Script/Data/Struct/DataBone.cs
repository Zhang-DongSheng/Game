using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataBone : DataBase
    {
        public List<Bone> bones = new List<Bone>();

        public SerializableDictionary<string, List<int>> dic = new SerializableDictionary<string, List<int>>();

        protected override void Editor()
        {

        }
    }
}