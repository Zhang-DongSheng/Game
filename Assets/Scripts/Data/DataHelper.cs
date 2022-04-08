namespace Data
{
    public static class DataHelper
    {
        static DataCurrency _currency;

        static DataProp _prop;

        static DataTask _task;

        public static DataCurrency Currency
        {
            get
            {
                if (_currency == null)
                {
                    DataManager.Instance.LoadAsync<DataCurrency>((asset) =>
                    {
                        _currency = asset;
                    });
                }
                return _currency;
            }
        }

        public static DataProp Prop
        {
            get
            {
                if (_prop == null)
                {
                    DataManager.Instance.LoadAsync<DataProp>((asset) =>
                    {
                        _prop = asset;
                    });
                }
                return _prop;
            }
        }

        public static DataTask Task
        {
            get
            {
                if (_task == null)
                {
                    DataManager.Instance.LoadAsync<DataTask>((asset) =>
                    {
                        _task = asset;
                    });
                }
                return _task;
            }
        }
    }
}