using Data.Serializable;
using System.Collections.Generic;

namespace Data
{
    public class DataBone : DataBase
    {
        public List<UnityTransform> bones = new List<UnityTransform>();

        protected override void Editor()
        {

        }
    }
}