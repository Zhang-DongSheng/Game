namespace Data
{
    public static class DataHelper
    {
        private static DataCurrency _currency;

        private static DataProp _prop;

        private static DataTask _task;

        public static DataCurrency Currency
        {
            get
            {
                if (_currency == null)
                {
                    _currency = DataManager.Instance.Load<DataCurrency>("Currency", "Data/Currency");
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
                    _prop = DataManager.Instance.Load<DataProp>("Prop", "Data/Prop");
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
                    _task = DataManager.Instance.Load<DataTask>("Task", "Data/Task");
                }
                return _task;
            }
        }
    }
}