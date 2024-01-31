using System.Collections.Generic;

namespace Game.UI
{
    public static class WarehouseConfig
    {
        public static readonly List<int> cate = new List<int>() { 1, 5 };
    }

    public enum WarehouseCategory
    {
        /// <summary>
        /// ÏûºÄÆ·
        /// </summary>
        Consume = 1,
        /// <summary>
        /// ±¦Ïä
        /// </summary>
        Chest = 2,
        /// <summary>
        /// ËéÆ¬
        /// </summary>
        Debris = 3,
    }
}
