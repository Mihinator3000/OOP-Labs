using System.Collections.Generic;
using System.Linq;
using Isu.Entities.GroupInfo;
using IsuExtra.Entities.Enums;
using IsuExtra.Entities.GroupInfo;
using IsuExtra.Entities.People;
using IsuExtra.Entities.Schedule;
using IsuExtra.Tools;
using GroupName = IsuExtra.Entities.GroupInfo.GroupName;

namespace IsuExtra.Services
{
    public class IsuExtraService
    {
        private readonly List<Group> _groups = new ();

        private readonly List<OgNp> _ogNps = new ();

        public Group AddGroup(GroupName name, MegaFaculties megaFaculty, WeekSchedule weekSchedule)
        {
            var group = new Group(name, megaFaculty, weekSchedule);
            _groups.Add(group);
            return group;
        }

        public Student AddStudent(GroupName groupName, string name)
        {
            Group group = _groups.First(u =>
                u.Name.IsEqual(groupName));

            var student = new Student(name, group);
            group.AddStudent(student);
            return student;
        }

        public OgNp AddOgNp(string name, MegaFaculties megaFaculty, WeekSchedule weekSchedule)
        {
            var ogNp = new OgNp(name, megaFaculty, weekSchedule);
            _ogNps.Add(ogNp);
            return ogNp;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            foreach (Group group in _groups)
            {
                if (!group.Students.Remove(student))
                    continue;

                int newGroupIndex = _groups.IndexOf(newGroup);
                student.ChangeGroup(_groups[newGroupIndex]);
                _groups[newGroupIndex].AddStudent(student);
                return;
            }

            throw new IsuExtraException("No student found");
        }

        public Group FindGroup(GroupName groupName)
        {
            return _groups.FirstOrDefault(u => u.Name.IsEqual(groupName));
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Where(u =>
                u.Name.CourseNumber.IsEqual(courseNumber)).ToList();
        }

        public Student GetStudent(string name)
        {
            foreach (Group group in _groups)
            {
                Student student = group.FindStudent(name);
                if (student != null)
                    return student;
            }

            throw new IsuExtraException("No student found");
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return _groups.First(u => u.Name.IsEqual(groupName)).Students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return _groups.First(u =>
                u.Name.CourseNumber.IsEqual(courseNumber)).Students;
        }

        public OgNp FindOgNp(string name)
        {
            return _ogNps.FirstOrDefault(u => u.Name == name);
        }

        public void AddOgNp(string studentName, OgNp ogNp)
        {
            Group group = _groups.FirstOrDefault(u =>
                u.Students.FirstOrDefault(s =>
                    s.Name == studentName) != null);

            if (group is null)
                throw new IsuExtraException("No student found");

            group.Students.First(u =>
                u.Name == studentName).AddOgNp(ogNp);
        }
    }
}
