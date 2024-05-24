using Data;
using Game.UI;
using System.Collections.Generic;

namespace Game
{
    public class WarehouseLogic : Logic<WarehouseLogic>
    {
        private readonly List<Prop> _props = new List<Prop>();

        private readonly List<Prop> _variables = new List<Prop>();

        protected override void OnRegister()
        {
            //NetworkMessageManager.Instance.Register(NetworkMessageDefine.C2s, OnReceivedInformation);
        }

        protected override void OnUnregister()
        {
            //NetworkMessageManager.Instance.Unregister(NetworkMessageDefine.C2s, OnReceivedInformation);
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

        public Prop GetProp(uint propID)
        {
            return _props.Find(x => x.identification == propID);
        }

        public int GetPropNumber(uint propID)
        {
            var prop = _props.Find(x => x.identification == propID);

            if (prop != null)
            {
                return prop.amount;
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
        private void OnReceivedInformation(object handle)
        {
            _props.Clear();

            DataProp data = DataManager.Instance.Load<DataProp>();

            for (int i = 0; i < data.list.Count; i++)
            {
                _props.Add(new Prop((uint)i, data.list[i].primary, 999));
            }
        }
        #endregion
    }
}