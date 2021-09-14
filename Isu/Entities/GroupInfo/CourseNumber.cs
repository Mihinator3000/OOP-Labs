using System;

namespace Isu.Entities.GroupInfo
{
    public class CourseNumber : IEqual<CourseNumber>
    {
        private readonly int _number;
        public CourseNumber(string number)
        {
            _number = Convert.ToInt32(number);
        }

        public bool IsEqual(CourseNumber other)
        {
            return _number == other._number;
        }
    }
}
