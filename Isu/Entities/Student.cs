using Isu.Entities.GroupInfo;

namespace Isu.Entities
{
    public class Student
    {
        private GroupName _groupName;

        public Student(string name, GroupName groupName)
        {
            Name = name;
            _groupName = groupName;
        }

        protected Student()
        {
        }

        public string Name { get; protected set; }

        public void ChangeGroup(GroupName groupName)
        {
            _groupName = groupName;
        }

        public bool IsInGroup(GroupName groupName)
        {
            return _groupName.IsEqual(groupName);
        }
    }
}
