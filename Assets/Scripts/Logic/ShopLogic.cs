using Data;
using System;
using System.Collections.Generic;

namespace Game
{
    public class ShopLogic : Singleton<ShopLogic>, ILogic
    {
        private readonly List<Counter> cabinets = new List<Counter>();

        public void Initialize()
        {
            NetworkEventManager.Register(NetworkEventKey.Shop, OnReceivedInformation);
        }

        public void Release()
        {
            NetworkEventManager.Unregister(NetworkEventKey.Shop, OnReceivedInformation);
        }

        public Counter Get(CounterCategory category)
        {
            return cabinets.Find(x => x.category == category);
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
            cabinets.Clear();

            foreach (var category in Enum.GetValues(typeof(CounterCategory)))
            {
                var counter = new Counter()
                {
                    category = (CounterCategory)category,
                    name = category.ToString(),
                    time = -1,
                    commodities = new List<Commodity>()
                };

                for (int i = 0; i < UnityEngine.Random.Range(5, 20); i++)
                {
                    counter.commodities.Add(new Commodity()
                    {
                        props = new List<Prop>()
                        {
                            new Prop()
                            {
                                identification = 10001,
                                number = 1,
                                parallelism = (uint)UnityEngine.Random.Range(10001,10020),
                            }
                        },
                        cost = new Cost()
                        {
                            consume = Consume.Currency,
                            coin = 101,
                            number = UnityEngine.Random.Range(50, 100),
                        },
                        status = Status.Available
                    });
                }
                cabinets.Add(counter);
            }
        }
        #endregion
    }
}