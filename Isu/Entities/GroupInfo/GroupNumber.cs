using System;

namespace Isu.Entities.GroupInfo
{
    public class GroupNumber : IEqual<GroupNumber>
    {
        public GroupNumber(string number)
        {
            Number = Convert.ToInt32(number);
        }

        public int Number { get; }

        public bool IsEqual(GroupNumber other)
        {
            return Number == other.Number;
        }
    }
}
