using Data;
using System.Collections.Generic;

namespace Game
{
    public class ShopLogic : Singleton<ShopLogic>, ILogic
    {
        private readonly List<Cabinet> cabinets = new List<Cabinet>();

        public void Init()
        {
            NetworkEventManager.Register(NetworkEventKey.Shop, OnReceivedInformation);
        }

        public Cabinet Get(int index)
        {
            return new Cabinet()
            {
                commodities = new List<Commodity>()
                {
                    new Commodity()
                    {
                        props = new List<Prop>()
                        { 
                            new Prop()
                            { 
                                
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