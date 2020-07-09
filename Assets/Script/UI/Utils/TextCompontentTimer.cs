using System;
using System.Text;
using TMPro;

namespace UnityEngine.UI
{
    public class TextCompontentTimer : MonoBehaviour
    {
        private readonly int Day = 86400, Hour = 3600, Minute = 60;

        private enum TimeType
        {
            Real,
            Game,
            Countdown,
            AlarmClock,
        }

        private enum TimeDisplay
        {
            Day_Hour_Minute_Second,
            Hour_Minute_Second,
            Minute_Second,
            Hour_Minute,
            TotalSecond,
        }

        public Action callBack;

        [SerializeField] private Text txt_time;

        [SerializeField] private TextMeshProUGUI tmp_time;

        [SerializeField] private TimeType timeType;
        
        [SerializeField] private TimeDisplay timeDisplay;
        
        [SerializeField] private float interval;

        [SerializeField] private bool breathe;

        private bool work;

        private bool respire;

        private float fixed_time;

        private float timer_update;

        private float timer_countdown;

        private float timer_breathe;

        private int total, day, hour, minute, second;

        private readonly StringBuilder builder = new StringBuilder();

        #region Core
        private void Awake()
        {
            if (txt_time == null)
                txt_time = GetComponent<Text>();
            if (tmp_time == null)
                tmp_time = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (!work) return;

            timer_update += Time.deltaTime;

            timer_countdown = fixed_time - Time.time;

            timer_breathe += Time.deltaTime;

            if (timer_update > interval)
            {
                timer_update = 0;

                switch (timeType)
                {
                    case TimeType.Real:
                        SetTime(DateTime.Now.ToString());
                        break;
                    case TimeType.Game:
                        SetTime(Time.time);
                        break;
                    case TimeType.Countdown:
                        SetTime(timer_countdown);
                        if (timer_countdown <= 0)
                        {
                            Completed();
                        }
                        break;
                    default:
                        break;
                }
            }

            if (breathe && timer_breathe > 1)
            {
                respire = !respire;

                timer_breathe = 0;
            }
        }

        private void SetTime(float totalSecond)
        {
            total = (int)totalSecond;

            day = (int)Math.Floor(totalSecond / Day);

            totalSecond -= day * Day;

            hour = (int)Math.Floor(totalSecond / Hour);

            totalSecond -= hour * Hour;

            minute = (int)Math.Floor(totalSecond / Minute);

            totalSecond -= minute * Minute;

            second = (int)totalSecond;

            builder.Clear();

            switch (timeDisplay)
            {
                case TimeDisplay.Day_Hour_Minute_Second:
                    builder.Append(day);
                    builder.Append(" ");
                    goto case TimeDisplay.Hour_Minute_Second;
                case TimeDisplay.Hour_Minute_Second:
                    builder.Append(FormatTime(hour));
                    builder.Append(":");
                    goto case TimeDisplay.Minute_Second;
                case TimeDisplay.Minute_Second:
                    builder.Append(FormatTime(minute));
                    builder.Append(breathe ? respire ? ":" : " " : ":");
                    builder.Append(FormatTime(second));
                    break;
                case TimeDisplay.Hour_Minute:
                    builder.Append(FormatTime(hour));
                    builder.Append(breathe ? respire ? ":" : " " : ":");
                    builder.Append(FormatTime(minute));
                    break;
                case TimeDisplay.TotalSecond:
                    builder.Append(total);
                    break;
            }

            SetTime(builder.ToString());
        }

        private void SetTime(string value)
        {
            if (txt_time != null)
                txt_time.text = value;
            if (tmp_time != null)
                tmp_time.text = value;
        }

        private string FormatTime(int time)
        {
            if (time < 10)
            {
                return string.Format("0{0}", time);
            }
            else
            {
                return time.ToString();
            }
        }

        private void Completed()
        {
            callBack?.Invoke(); work = false;
        }
        #endregion

        #region Function
        public void Stop()
        {
            work = false;
        }

        public void Countdown(float time, Action action = null)
        {
            timeType = TimeType.Countdown;

            callBack = action;

            fixed_time = Time.time + time;

            work = true;
        }

        public void SetText(string value)
        {
            if (txt_time != null)
                txt_time.text = value;
            if (tmp_time != null)
                tmp_time.text = value;
        }
        #endregion
    }
}