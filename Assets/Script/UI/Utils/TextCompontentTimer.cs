using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class TextCompontentTimer : MonoBehaviour
    {
        private const int Day = 86400, Hour = 3600, Minute = 60;

        enum TimeType
        {
            Real,
            Game,
            Countdown,
        }

        enum TimeDisplay
        {
            SMHDMY,
            SMHDM,
            SMHD,
            SMH,
            SM,
            S,
        }

        public Action callBack;

        [SerializeField] private TimeType type;

        [SerializeField] private TimeDisplay display;

        [SerializeField] private Text txt_second;

        [SerializeField] private Text txt_minute;

        [SerializeField] private Text txt_hour;

        [SerializeField] private Text txt_day;

        [SerializeField] private List<GameObject> obj_respire;

        [SerializeField] private float interval;

        [SerializeField] private bool breathe;

        private bool work;

        private bool respire;

        private float terminal;

        private float timer_update;

        private float timer_countdown;

        private float timer_breathe;

        private int total, day, hour, minute, second;

        #region Core
        private void Update()
        {
            if (!work) return;

            timer_update += Time.deltaTime;

            timer_breathe += Time.deltaTime;

            timer_countdown = terminal - Time.time;

            if (timer_update > interval)
            {
                timer_update = 0;

                switch (type)
                {
                    case TimeType.Real:
                        SetText(DateTime.Now);
                        break;
                    case TimeType.Game:
                        SetText(Time.time);
                        break;
                    case TimeType.Countdown:
                        SetText(timer_countdown);
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
                respire = !respire; timer_breathe = 0;

                SetRespire(respire);
            }
        }

        private void Completed()
        {
            callBack?.Invoke();
            
            work = false;
        }
        #endregion

        #region Function
        public void CountDown(float time, Action action = null)
        {
            type = TimeType.Countdown;

            callBack = action;

            terminal = Time.time + time;

            work = true;

            gameObject.SetActive(true);
        }

        public void StartUp()
        {
            work = true;

            gameObject.SetActive(true);
        }

        public void Stop()
        {
            work = false;

            gameObject.SetActive(false);
        }
        #endregion

        #region Refresh
        private void SetText(DateTime time)
        {
            SetText(time.Second, time.Minute, time.Hour, time.Day, time.Month, time.Year);
        }

        private void SetText(float totalSecond)
        {
            total = (int)totalSecond;

            day = (int)Math.Floor(totalSecond / Day);

            totalSecond -= day * Day;

            hour = (int)Math.Floor(totalSecond / Hour);

            totalSecond -= hour * Hour;

            minute = (int)Math.Floor(totalSecond / Minute);

            totalSecond -= minute * Minute;

            second = (int)totalSecond;

            if (display != TimeDisplay.S)
            {
                SetText(second, minute, hour, day);
            }
            else
            {
                SetText(total);
            }
        }

        private void SetText(int second, int minute = 0, int hour = 0, int day = 0, int month = 0, int year = 0)
        {
            switch (display)
            {
                case TimeDisplay.S:
                    SetText(txt_second, second, false);
                    break;
                case TimeDisplay.SM:
                    SetText(txt_second, second);
                    SetText(txt_minute, minute);
                    break;
                case TimeDisplay.SMH:
                    SetText(txt_second, second);
                    SetText(txt_minute, minute);
                    SetText(txt_hour, hour);
                    break;
                case TimeDisplay.SMHD:
                    SetText(txt_second, second);
                    SetText(txt_minute, minute);
                    SetText(txt_hour, hour);
                    SetText(txt_day, day);
                    break;
                case TimeDisplay.SMHDM:
                    SetText(txt_second, second);
                    SetText(txt_minute, minute);
                    SetText(txt_hour, hour);
                    SetText(txt_day, string.Format("{0} {1}", month, day));
                    break;
                case TimeDisplay.SMHDMY:
                    SetText(txt_second, second);
                    SetText(txt_minute, minute);
                    SetText(txt_hour, hour);
                    SetText(txt_day, string.Format("{0}.{1} {2}", year, month, day));
                    break;
                default:
                    break;
            }
        }

        private void SetText(Text compontent, int number, bool cover = true)
        {
            string content = cover && number < 10 ? string.Format("0{0}", number) : number.ToString();

            SetText(compontent, content);
        }

        private void SetText(Text compontent, string content)
        {
            if (compontent != null)
                compontent.text = content;
        }

        private void SetRespire(bool active)
        {
            for (int i = 0; i < obj_respire.Count; i++)
            {
                if (obj_respire[i] != null && obj_respire[i].activeSelf != active)
                {
                    obj_respire[i].SetActive(active);
                }
            }
        }
        #endregion
    }
}