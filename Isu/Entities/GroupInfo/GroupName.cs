using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Entities.GroupInfo
{
    public class GroupName : IEqual<GroupName>
    {
        private readonly GroupNumber _groupNumber;

        public GroupName(string name)
        {
            var regex = new Regex(@"^M3(\d)(\d{2})$");
            Match match = regex.Match(name);
            if (!match.Success)
            {
                throw new IsuException(name);
            }

            CourseNumber = new CourseNumber(match.Groups[1].Value);
            _groupNumber = new GroupNumber(match.Groups[2].Value);
        }

        public CourseNumber CourseNumber { get; }

        public bool IsEqual(GroupName other)
        {
            return _groupNumber.IsEqual(other._groupNumber) &&
                   CourseNumber.IsEqual(other.CourseNumber);
        }
    }
}
