using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataBone : DataBase
    {
        public List<SerializableBone> bones = new List<SerializableBone>();

        protected override void Editor()
        {

        }
    }
}