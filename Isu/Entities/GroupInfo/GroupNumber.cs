using System;

namespace Isu.Entities.GroupInfo
{
    public class GroupNumber : IEqual<GroupNumber>
    {
        private readonly int _number;
        public GroupNumber(string number)
        {
            _number = Convert.ToInt32(number);
        }

        public bool IsEqual(GroupNumber other)
        {
            return _number == other._number;
        }
    }
}
