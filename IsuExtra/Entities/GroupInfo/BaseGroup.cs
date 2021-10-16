using System.Collections.Generic;
using System.Linq;
using IsuExtra.Entities.Enums;
using IsuExtra.Entities.People;
using IsuExtra.Entities.Schedule;

namespace IsuExtra.Entities.GroupInfo
{
    public abstract class BaseGroup
    {
        protected BaseGroup(GroupName name, MegaFaculties megaFaculty, WeekSchedule schedule)
        {
            Group = new Isu.Entities.Group(name);
            MegaFaculty = megaFaculty;
            Schedule = schedule;
        }

        public MegaFaculties MegaFaculty { get; }

        public WeekSchedule Schedule { get; }

        public GroupName Name => (GroupName)Group.Name;

        public List<Student> Students =>
            Group.Students.Cast<Student>().ToList();

        protected Isu.Entities.Group Group { get; }

        public void AddStudent(Student student)
        {
            Group.AddStudent(student);
        }
    }
}
