using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Entities.GroupInfo
{
    public class GroupName : IEqual<GroupName>
    {
        public GroupName(CourseNumber courseNumber, GroupNumber groupNumber)
        {
            CourseNumber = courseNumber;
            GroupNumber = groupNumber;
        }

        public GroupName(string name)
        {
            Match match = new Regex(@"^M3(\d)(\d{2})$").Match(name);
            if (!match.Success)
            {
                throw new IsuException(name);
            }

            CourseNumber = new CourseNumber(match.Groups[1].Value);
            GroupNumber = new GroupNumber(match.Groups[2].Value);
        }

        protected GroupName()
        {
        }

        public CourseNumber CourseNumber { get; protected set; }

        public GroupNumber GroupNumber { get; protected set; }

        public bool IsEqual(GroupName other)
        {
            return GroupNumber.IsEqual(other.GroupNumber) &&
                   CourseNumber.IsEqual(other.CourseNumber);
        }
    }
}
