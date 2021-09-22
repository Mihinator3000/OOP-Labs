using System.Collections.Generic;
using System.Linq;
using Isu.Entities.GroupInfo;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private const int MaxStudentPerGroup = 30;
        public Group(GroupName name)
        {
            Name = name;
        }

        public GroupName Name { get; }

        public List<Student> Students { get; } = new ();

        public void AddStudent(Student student)
        {
            Students.Add(student);
            if (Students.Count > MaxStudentPerGroup)
                throw new IsuException();
        }

        public Student FindStudent(string name)
        {
            return Students.FirstOrDefault(u => u.Name == name);
        }

        public bool Contains(string name)
        {
            return FindStudent(name) != null;
        }
    }
}
