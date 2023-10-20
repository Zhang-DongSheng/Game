using Data;
using System.Collections.Generic;

namespace Game
{
    public class WarehouseLogic : Singleton<WarehouseLogic>, ILogic
    {
        private readonly List<Prop> _props = new List<Prop>();

        private readonly List<Prop> _variables = new List<Prop>();

        public List<Prop> Props
        {
            get { return _props; }
        }

        public void Initialize()
        {
            NetworkEventManager.Register(NetworkEventKey.Warehouse, OnReceivedInformation);
        }

        public void Release()
        {
            NetworkEventManager.Unregister(NetworkEventKey.Warehouse, OnReceivedInformation);
        }

        public List<Prop> GetPropsByCategory(int category)
        {
            _variables.Clear();

            DataProp data = DataManager.Instance.Load<DataProp>();

            PropInformation prop;

            int count = _props.Count;

            for (int i = 0; i < count; i++)
            {
                prop = data.Get(_props[i].parallelism);

                if (prop != null && prop.category == category)
                {
                    _variables.Add(_props[i]);
                }
            }
            return _variables;
        }

        public void Renovate(Prop prop)
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

        public Prop GetProp(int propID)
        {
            return _props.Find(x => x.identification == propID);
        }

        public uint GetPropNumber(int propID)
        {
            var prop = _props.Find(x => x.identification == propID);

            if (prop != null)
            {
                return prop.number;
            }
            return 0;
        }

        #region Request
        public void RequestInformation()
        {
            OnReceivedInformation(null);

            ScheduleLogic.Instance.Update(Schedule.Bag);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {
            _props.Clear();

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
        #endregion
    }
}