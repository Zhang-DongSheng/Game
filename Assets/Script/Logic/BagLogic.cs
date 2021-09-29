using Data;
using System.Collections.Generic;

namespace Game
{
    public class BagLogic : Singleton<BagLogic>, ILogic
    {
        private readonly List<Currency> currencies = new List<Currency>();

        private readonly List<Prop> props = new List<Prop>();

        public void Init()
        {
            NetworkEventManager.Register(NetworkEventKey.Bag, OnReceivedInformation);
        }

        #region Function
        public void RenovateCurrency(CurrencyType currency, int number)
        {
            if (currencies.Exists(x => x.currency == currency))
            {
                for (int i = 0; i < currencies.Count; i++)
                {
                    if (currencies[i].currency == currency)
                    {
                        currencies[i].number = number;
                        break;
                    }
                }
            }
            else
            {
                currencies.Add(new Currency(currency, number));
            }
        }

        public void RenovateProp(Prop prop)
        {
            if (props.Exists(x => x.identification == prop.identification))
            {
                for (int i = 0; i < props.Count; i++)
                {
                    if (props[i].identification == prop.identification)
                    {
                        props[i] = prop;
                        break;
                    }
                }
            }
            else
            {
                props.Add(prop);
            }
        }
        #endregion

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