using System.Collections.Generic;
using System.Linq;
using IsuExtra.Entities.Enums;
using IsuExtra.Entities.People;
using IsuExtra.Entities.Schedule;
using IsuExtra.Tools;

namespace IsuExtra.Entities.GroupInfo
{
    public class Group
    {
        private const int MaxStudentPerGroup = 30;

        public Group(GroupName name)
        {
            Name = name;
        }

        public Group(GroupName name, MegaFaculties megaFaculty)
            : this(name)
        {
            MegaFaculty = megaFaculty;
        }

        public Group(GroupName name, MegaFaculties megaFaculty, WeekSchedule schedule)
            : this(name, megaFaculty)
        {
            Schedule = schedule;
        }

        public GroupName Name { get; }

        public MegaFaculties MegaFaculty { get; }

        public List<Student> Students { get; } = new ();

        public WeekSchedule Schedule { get; }

        public void AddStudent(Student student)
        {
            Students.Add(student);
            if (Students.Count > MaxStudentPerGroup)
                throw new IsuExtraException();
        }

        public Student FindStudent(string name)
        {
            return Students.FirstOrDefault(u => u.Name == name);
        }

        public bool Contains(string name)
        {
            return FindStudent(name) != null;
        }

        public List<Student> DoNotHaveOgNp()
        {
            return Students.Where(u =>
                u.DoesNotHaveAnyOgNp()).ToList();
        }
    }
}
