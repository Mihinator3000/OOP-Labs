using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Isu.Entities;
using Isu.Entities.GroupInfo;
using Isu.Tools;
using Group = Isu.Entities.Group;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups = new ();

        public Group AddGroup(GroupName name)
        {
            var group = new Group(name);
            _groups.Add(group);
            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            var student = new Student(name, group.Name);
            int groupIndex = _groups.IndexOf(group);
            _groups[groupIndex].AddStudent(student);
            return student;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            foreach (Group group in _groups)
            {
                if (!group.Students.Remove(student))
                    continue;
                int newGroupIndex = _groups.IndexOf(newGroup);
                student.ChangeGroup(_groups[newGroupIndex].Name);
                _groups[newGroupIndex].AddStudent(student);
                return;
            }
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

        public Student FindStudent(string name)
        {
            foreach (Group group in _groups)
            {
                Student student = group.FindStudent(name);
                if (student != null)
                    return student;
            }

            throw new IsuException();
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

        public Student GetStudent(int id)
        {
            Match match = new Regex(@"^M3(\d{3})(\d+)$").Match(id.ToString());
            if (!match.Success)
            {
                throw new IsuException(id.ToString());
            }

            Group group = FindGroup(new GroupName($"M3{match.Groups[1]}"));

            int index = Convert.ToInt32(match.Groups[2]);
            if (group.Students.Count < index)
            {
                throw new IsuException(index.ToString());
            }

            return group.Students[index];
        }
    }
}
