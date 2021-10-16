using System.Collections.Generic;
using System.Linq;
using IsuExtra.Entities.Enums;
using IsuExtra.Entities.People;
using IsuExtra.Entities.Schedule;

namespace IsuExtra.Entities.GroupInfo
{
    public class Group : BaseGroup
    {
        public Group(GroupName name, MegaFaculties megaFaculty, WeekSchedule schedule)
            : base(name, megaFaculty, schedule)
        {
        }

        public Student FindStudent(string name)
        {
            return (Student)Group.FindStudent(name);
        }

        public bool Contains(string name)
        {
            return Group.Contains(name);
        }

        public List<Student> DoNotHaveOgNp()
        {
            return Students.Where(u =>
                u.DoesNotHaveAnyOgNp()).ToList();
        }
    }
}
