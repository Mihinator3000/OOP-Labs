using IsuExtra.Entities.People;

namespace IsuExtra.Entities.Schedule
{
    public class Lesson
    {
        public Lesson(
            string name,
            string time,
            string teacherName,
            string auditory)
        {
            Name = name;
            Time = new ClassTime(time);
            CurrentTeacher = new Teacher(teacherName);
            Auditory = auditory;
        }

        public string Name { get; }

        public ClassTime Time { get; }

        public Teacher CurrentTeacher { get; }

        public string Auditory { get; }

        public bool DoesIntersect(Lesson other) =>
            Time.DoesIntersect(other.Time);
    }
}