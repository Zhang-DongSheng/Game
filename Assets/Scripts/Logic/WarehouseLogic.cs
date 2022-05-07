using Data;
using System.Collections.Generic;

namespace Game
{
    public class WarehouseLogic : Singleton<WarehouseLogic>, ILogic
    {
        private readonly List<Currency> _currencies = new List<Currency>();

        private readonly List<Prop> _props = new List<Prop>();

        private readonly List<Prop> _column = new List<Prop>();

        public List<Currency> Currencies
        {
            get { return _currencies; }
        }

        public List<Prop> Props
        {
            get { return _props; }
        }

        public void Init()
        {
            NetworkEventManager.Register(NetworkEventKey.Warehouse, OnReceivedInformation);

            DataProp data = DataManager.Instance.Load<DataProp>();

            for (int i = 0; i < data.props.Count; i++)
            {
                _props.Add(new Prop()
                {
                    identification = (uint)i,
                    number = 999,
                    parallelism = data.props[i].primary,
                });
            }
        }

        public List<Prop> Column(int index)
        {
            _column.Clear();

            DataProp data = DataManager.Instance.Load<DataProp>();

            PropInformation prop;

            int count = _props.Count;

            for (int i = 0; i < count; i++)
            {
                prop = data.Get(_props[i].parallelism);

                if (prop != null && prop.category == index)
                {
                    _column.Add(_props[i]);
                }
            }
            return _column;
        }

        #region Function
        public void RenovateCurrency(CurrencyEnum currency, int number)
        {
            if (_currencies.Exists(x => x.currency == currency))
            {
                for (int i = 0; i < _currencies.Count; i++)
                {
                    if (_currencies[i].currency == currency)
                    {
                        _currencies[i].number = number;
                        break;
                    }
                }
            }
            else
            {
                _currencies.Add(new Currency(currency, number));
            }
        }

        public void RenovateProp(Prop prop)
        {
            if (_props.Exists(x => x.identification == prop.identification))
            {
                for (int i = 0; i < _props.Count; i++)
                {
                    if (_props[i].identification == prop.identification)
                    {
                        _props[i] = prop;
                        break;
                    }
                }
            }
            else
            {
                _props.Add(prop);
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