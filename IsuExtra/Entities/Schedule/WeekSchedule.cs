using System.Collections.Generic;
using IsuExtra.Tools;

namespace IsuExtra.Entities.Schedule
{
    public class WeekSchedule
    {
        private const int DaysInAWeek = 7;
        public WeekSchedule(IReadOnlyCollection<DaySchedule> schedule)
        {
            if (schedule.Count > DaysInAWeek)
                throw new IsuExtraException("Invalid schedule");

            for (int i = 0; i < DaysInAWeek; i++)
                Schedule.Add(null);

            foreach (DaySchedule daySchedule in schedule)
            {
                int index = (int)daySchedule.DayOfTheWeek;
                if (Schedule[index] != null)
                    throw new IsuExtraException("Invalid DayOfTheWeek input");

                Schedule[index] = daySchedule;
            }
        }

        public List<DaySchedule> Schedule { get; } = new ();

        public bool DoesIntersect(WeekSchedule other)
        {
            for (int i = 0; i < DaysInAWeek; i++)
            {
                if (Schedule[i] is null
                    || other.Schedule[i] is null)
                    continue;

                if (Schedule[i].DoesIntersect(
                    other.Schedule[i]))
                    return true;
            }

            return false;
        }
    }
}