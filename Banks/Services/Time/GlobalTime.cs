using System;

namespace Banks.Services.Time
{
    public class GlobalTime
    {
        private static GlobalTime _instance;

        private TimeSpan _timeShift;

        private GlobalTime()
        {
            _timeShift = TimeSpan.Zero;
        }

        public static DateTime Now =>
            DateTime.Now.Add(GetInstance._timeShift);

        private static GlobalTime GetInstance =>
            _instance ??= new GlobalTime();

        public static void AddTime(TimeSpan time)
        {
           GetInstance._timeShift += time;
        }
    }
}