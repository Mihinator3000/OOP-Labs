using Isu.Entities.GroupInfo;

namespace Isu.Entities
{
    public class Student : IEqual<Student>
    {
        private GroupName _groupName;
        public Student(string name, GroupName groupName)
        {
            Name = name;
            _groupName = groupName;
        }

        public string Name { get; }

        public void ChangeGroup(GroupName groupName)
        {
            _groupName = groupName;
        }

        public bool IsInGroup(GroupName groupName)
        {
            return _groupName.IsEqual(groupName);
        }

        public bool IsEqual(Student other)
        {
            return Name == other.Name;
        }
    }
}
