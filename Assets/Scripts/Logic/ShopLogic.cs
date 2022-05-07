using Data;
using System.Collections.Generic;

namespace Game
{
    public class ShopLogic : Singleton<ShopLogic>, ILogic
    {
        private readonly List<Counter> cabinets = new List<Counter>();

        public void Init()
        {
            NetworkEventManager.Register(NetworkEventKey.Shop, OnReceivedInformation);
        }

        public Counter Get(CounterCategory category)
        {
            Counter counter = cabinets.Find(x => x.category == category);

            if (counter == null)
            {
                counter = new Counter()
                {
                    category = category,
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
                                identification = 1001,
                                number = 1,
                                parallelism = (uint)UnityEngine.Random.Range(0,4),
                            }
                        }
                    });
                }
                cabinets.Add(counter);
            }
            return counter;
        }

        #region Request
        public void RequestInformation()
        {

        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {

        }
        #endregion
    }
}