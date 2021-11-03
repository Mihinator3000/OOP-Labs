using System;

namespace Banks.Services.Time
{
    public class GlobalTime
    {
        private static GlobalTime _instance;

        private DateTime _dateTime;

        private GlobalTime()
        {
            _dateTime = DateTime.Now;
        }

        public static DateTime Now
        {
            get => GetInstance._dateTime;
            private set => GetInstance._dateTime = value;
        }

        private static GlobalTime GetInstance
        {
            get
            {
                return _instance ??= new GlobalTime();
            }
        }

        public static void RewindTime(TimeSpan time)
        {
            Now -= time;
        }

        public static void AddTime(TimeSpan time)
        {
            Now += time;
        }
    }
}