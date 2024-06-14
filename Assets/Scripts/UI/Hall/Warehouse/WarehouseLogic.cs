using Data;
using Game.Network;
using Protobuf;
using System.Collections.Generic;

namespace Game.UI
{
    public class WarehouseLogic : Singleton<WarehouseLogic>, ILogic
    {
        private readonly List<Prop> _props = new List<Prop>();

        private readonly List<Prop> _variables = new List<Prop>();

        public void Initialize()
        {

        }

        public List<Prop> GetPropsByCategory(int category)
        {
            _variables.Clear();

            int count = _props.Count;

            for (int i = 0; i < count; i++)
            {
                var prop = DataProp.Get(_props[i].parallelism);

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

        public Prop GetProp(long propID)
        {
            return _props.Find(x => x.identification == propID);
        }

        public Prop GetProp(uint propID)
        {
            return _props.Find(x => x.parallelism == propID);
        }

        public uint GetPropNumber(long propID)
        {
            var prop = _props.Find(x => x.identification == propID);

            if (prop != null)
            {
                return prop.amount;
            }
            return 0;
        }

        public uint GetPropNumber(uint propID)
        {
            var prop = _props.Find(x => x.parallelism == propID);

            if (prop != null)
            {
                return prop.amount;
            }
            return 0;
        }

        public void RequestInformation()
        {
            var msg = new C2SWarehouseRequest();

            NetworkManager.Instance.Send(NetworkMessageDefine.C2SWarehouseRequest, msg, (handle) =>
            {
                _props.Clear();

                DataProp data = DataManager.Instance.Load<DataProp>();

                for (int i = 0; i < data.list.Count; i++)
                {
                    _props.Add(new Prop((uint)i, data.list[i].primary, 999));
                }
                ScheduleLogic.Instance.Update(Schedule.Bag);
            });
        }
    }
}