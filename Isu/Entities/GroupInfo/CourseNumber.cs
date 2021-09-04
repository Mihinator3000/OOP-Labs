using System;

namespace Isu.Entities.GroupInfo
{
    public class CourseNumber : IEqual<CourseNumber>
    {
        public CourseNumber(string number)
        {
            Number = Convert.ToInt32(number);
        }

        public int Number { get; }

        public bool IsEqual(CourseNumber other)
        {
            return Number == other.Number;
        }
    }
}
