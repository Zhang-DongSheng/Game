using Data;
using System.Collections.Generic;

namespace Game
{
    public class ShopLogic : Singleton<ShopLogic>, ILogic
    {
        private readonly List<Shop> _shops = new List<Shop>();

        public void Initialize()
        {
            NetworkEventManager.Register(NetworkEventKey.Shop, OnReceivedInformation);
        }

        public void Release()
        {
            NetworkEventManager.Unregister(NetworkEventKey.Shop, OnReceivedInformation);
        }

        public bool Exists(int shop)
        {
            return _shops.Exists(x => x.shop == shop);
        }

        public Shop Get(int shop)
        {
            return _shops.Find(x => x.shop == shop);
        }

        #region Request
        public void RequestInformation()
        {
            OnReceivedInformation(null);

            ScheduleLogic.Instance.Update(Schedule.Shop);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {
            var array = new List<int>() { 101, 102, 103 };

            _shops.Clear();

            foreach (var shop in array)
            {
                var counter = new Shop()
                {
                    shop = shop,
                    name = shop.ToString(),
                    time = -1,
                    commodities = new List<Commodity>()
                };

                for (int i = 0; i < UnityEngine.Random.Range(5, 10); i++)
                {
                    counter.commodities.Add(new Commodity()
                    {
                        identification = 1000 + (uint)i,
                        primary = 1001 + (uint)i,
                        status = Status.Available
                    });
                }
                _shops.Add(counter);
            }
        }
        #endregion
    }
}