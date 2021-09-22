using System.Collections.Generic;
using IsuExtra.Entities.Enums;
using IsuExtra.Entities.People;
using IsuExtra.Entities.Schedule;
using IsuExtra.Tools;

namespace IsuExtra.Entities.Subjects
{
    public class OgNp
    {
        public const int MaxPersonPerOgNp = 20;

        private readonly List<Student> _students = new ();

        public OgNp(string name, MegaFaculties megaFaculties, WeekSchedule weekSchedule)
        {
            Name = name;
            MegaFaculty = megaFaculties;
            Schedule = weekSchedule;
        }

        public string Name { get; }

        public WeekSchedule Schedule { get; }

        public MegaFaculties MegaFaculty { get; }

        public void AddStudent(Student student)
        {
            if (_students.Count > MaxPersonPerOgNp)
                throw new IsuExtraException("Students overflow");

            _students.Add(student);
        }

        public List<Student> EnrolledStudents() => _students;
    }
}