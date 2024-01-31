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
        /// ����Ʒ
        /// </summary>
        Consume = 1,
        /// <summary>
        /// ����
        /// </summary>
        Chest = 2,
        /// <summary>
        /// ��Ƭ
        /// </summary>
        Debris = 3,
    }
}
