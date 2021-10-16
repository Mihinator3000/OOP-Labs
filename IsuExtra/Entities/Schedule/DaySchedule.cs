using System.Collections.Generic;
using System.Linq;
using IsuExtra.Entities.Enums;

namespace IsuExtra.Entities.Schedule
{
    public class DaySchedule
    {
        public DaySchedule(DaysOfTheWeek dayOfTheWeek)
        {
            DayOfTheWeek = dayOfTheWeek;
        }

        public DaysOfTheWeek DayOfTheWeek { get; }

        public List<Lesson> Schedule { get; init; }

        public bool DoesIntersect(DaySchedule daySchedule)
        {
            return daySchedule.Schedule.Any(u =>
                Schedule.Any(o =>
                    o.DoesIntersect(u)));
        }
    }
}