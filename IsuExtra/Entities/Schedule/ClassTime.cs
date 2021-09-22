using System;
using System.Text.RegularExpressions;
using IsuExtra.Tools;

namespace IsuExtra.Entities.Schedule
{
    public class ClassTime
    {
        private const int MaxHours = 24;
        private const int MaxMinutes = 60;

        private static readonly TimeSpan Duration = TimeSpan.FromHours(1.5);

        private TimeSpan _startTime;

        public ClassTime(string time)
        {
            if (!TryInitialize(time))
                throw new IsuExtraException("Incorrect time input");
        }

        public bool DoesIntersect(ClassTime other)
        {
            return _startTime < other._startTime
                ? _startTime.Add(Duration) >= other._startTime
                : other._startTime.Add(Duration) >= _startTime;
        }

        private bool TryInitialize(string time)
        {
            Match match = new Regex(@"^(\d{1,2}):(\d{2})$").Match(time);
            if (!match.Success)
                return false;

            int hours = Convert.ToInt32(match.Groups[1].Value);
            int minutes = Convert.ToInt32(match.Groups[2].Name);

            if (hours >= MaxHours
                || minutes >= MaxMinutes)
                return false;

            _startTime = new TimeSpan(hours, minutes, 0);
            return true;
        }
    }
}