namespace BlazorApp1.ZigbeeModels
{
    public class WeeklySchedule : ZigbeeDevice
    {
        public event EventHandler<int>? CalenderActive;
        public event EventHandler<int>? CalenderInactive;
        public List<WeekDay> WeekDays;
        private System.Timers.Timer timer;
        private bool active;
        public WeeklySchedule()
        {
            WeekDays = new List<WeekDay>()
            {
                new WeekDay(DayOfWeek.Monday),
                new WeekDay(DayOfWeek.Tuesday),
                new WeekDay(DayOfWeek.Wednesday),
                new WeekDay(DayOfWeek.Thursday),
                new WeekDay(DayOfWeek.Friday),
                new WeekDay(DayOfWeek.Saturday),
                new WeekDay(DayOfWeek.Sunday)
            };
            timer = new(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
            active = false;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            DayOfWeek dag = DateTime.Now.DayOfWeek;
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            WeekDay? selectedDay = SelectWeekDay(dag);
            int count = selectedDay.OnOrOffTime.Count;
            double firstNum = 0, nextNum = 0, interpolatedValue = 0;
            TimeOnly firstTime, nextTime;


            // skal kalenderen tænde eller slukke?
            if (count == 0)
            {
                RaiseCalenderEvent(0);
                return;               // hold værdi fra dagen før
            }

            foreach (TimeAction timeAction in selectedDay.OnOrOffTime)
            {
                if (timeAction.Time < time)
                {
                    active = timeAction.IsActive;           // sidste tid før actuel tid bruges
                    firstNum = timeAction.Value;
                    firstTime = timeAction.Time;
                    nextNum = timeAction.Value;
                    nextTime = timeAction.Time;
                }
                else
                {
                    nextNum = timeAction.Value;
                    nextTime = timeAction.Time;
                    break;
                }
            }

            interpolatedValue = CalculateInterpolatedValue(firstNum, nextNum, firstTime, nextTime, time);

            RaiseCalenderEvent((int)interpolatedValue);
        }

        private double CalculateInterpolatedValue(double firstNum, double nextNum, TimeOnly firstTime, TimeOnly nextTime, TimeOnly time)
        {
            double interpolatedValue = 0;
            double howFar;
            double distance = nextNum - firstNum;
            TimeSpan timeSpan = nextTime - firstTime;
            TimeSpan offsetTime = time - firstTime;

            if (timeSpan == TimeSpan.Zero) return firstNum;

            howFar = offsetTime.TotalMilliseconds / timeSpan.TotalMilliseconds;
            interpolatedValue = firstNum + howFar * distance;

            return interpolatedValue;
        }

        private void RaiseCalenderEvent(int interpolatedValue)
        {
            if (active)
            {
                CalenderActive?.Invoke(this, interpolatedValue);
            }
            else
            {
                CalenderInactive?.Invoke(this, interpolatedValue);
            }
        }


        public void AddScheduleItem(DayOfWeek day, TimeOnly time, bool onOrOff, float value)
        {
            WeekDay? selectedDay = SelectWeekDay(day);
            if (selectedDay == null) return;
            TimeAction action = new() { Time = time, IsActive = onOrOff, Value = value };
            selectedDay.AddOnOrOffTime(action);
        }
        public void RemoveScheduleItem(DayOfWeek day, TimeOnly time)
        {
            WeekDay? selectedDay = SelectWeekDay(day);
            if (selectedDay == null) return;
            selectedDay.RemoveOnOrOffTime(time);
        }

        public void EnableDay(DayOfWeek day)
        {
            WeekDay? selectedDay = SelectWeekDay(day);
            if (selectedDay == null) return;
            selectedDay.Enabled = true;
        }

        public void DisableDay(DayOfWeek day)
        {
            WeekDay? selectedDay = SelectWeekDay(day);
            if (selectedDay == null) return;
            selectedDay.Enabled = false;
        }

        private WeekDay? SelectWeekDay(DayOfWeek day)
        {
            return WeekDays.FirstOrDefault(x => x.Day == day);
        }
    }

    public class TimerEventArgs : EventArgs
    {
        public double Value { get; set; }
    }

    public class WeekDay
    {
        public DayOfWeek Day { get; set; }
        public bool Enabled { get; set; }
        public List<TimeAction> OnOrOffTime { get; set; }

        public WeekDay(DayOfWeek day)
        {
            Day = day;
            Enabled = false;
            OnOrOffTime = new List<TimeAction>();
        }
        public void AddOnOrOffTime(TimeAction time)
        {
            OnOrOffTime.Add(time);
            OnOrOffTime.Sort((x, y) => x.Time.CompareTo(y.Time));
        }

        public void RemoveOnOrOffTime(TimeOnly time)
        {
            OnOrOffTime = OnOrOffTime.Where(x => x.Time != time).ToList();
        }
        public void ClearDay()
        {
            OnOrOffTime.Clear();
        }
    }

    public class TimeAction
    {
        public TimeOnly Time { get; set; } = new TimeOnly();
        public bool IsActive { get; set; } = false;
        public double Value { get; set; } = 0;
    }


}
