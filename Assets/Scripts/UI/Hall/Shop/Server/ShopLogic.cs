using Game.Data;
using Game.Network;
using Protobuf;
using System.Collections.Generic;

namespace Game.Logic
{
    public class ShopLogic : Singleton<ShopLogic>, ILogic
    {
        private readonly List<Shop> _shops = new List<Shop>();

        public void Initialize()
        {

        }

        public bool Exists(int shop)
        {
            return _shops.Exists(x => x.shop == shop);
        }

        public Shop Get(int shop)
        {
            return _shops.Find(x => x.shop == shop);
        }

        public void RequestInformation()
        {
            var msg = new C2SShopRequest()
            {

            };
            NetworkManager.Instance.Send(NetworkMessageDefine.C2SShopRequest, msg, (handle) =>
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
                            purchased = 0
                        });
                    }
                    _shops.Add(counter);
                }
                ScheduleLogic.Instance.Update(Schedule.Shop);
            });
        }

        #region Receive
        private void OnReceivedInformation(object handle)
        {
            
        }
        #endregion
    }
}