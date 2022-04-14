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

        public Counter Get(int index)
        {
            return new Counter()
            {
                commodities = new List<Commodity>()
                {
                    new Commodity()
                    {
                        props = new List<Prop>()
                        {
                            new Prop()
                            {
                                identification = 1001,
                                number = 1,
                                parallelism = UnityEngine.Random.Range(0,4),
                            }
                        }
                    }
                }
            };
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