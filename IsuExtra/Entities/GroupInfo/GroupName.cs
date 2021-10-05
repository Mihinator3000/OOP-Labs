using System;
using System.Text.RegularExpressions;
using Isu.Entities.GroupInfo;
using IsuExtra.Tools;

namespace IsuExtra.Entities.GroupInfo
{
    public class GroupName : Isu.Entities.GroupInfo.GroupName
    {
        private readonly char _facultySymbol;
        public GroupName(string name)
        {
            Match match = new Regex(@"^([a-zA-Z])(\d{2})(\d{2})$").Match(name);
            if (!match.Success)
            {
                throw new IsuExtraException(name);
            }

            _facultySymbol = Convert.ToChar(match.Groups[1].Value);
            CourseNumber = new CourseNumber(match.Groups[2].Value);
            GroupNumber = new GroupNumber(match.Groups[3].Value);
        }
    }
}