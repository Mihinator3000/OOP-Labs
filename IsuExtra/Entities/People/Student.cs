using System.Collections.Generic;
using System.Linq;
using IsuExtra.Entities.GroupInfo;
using IsuExtra.Tools;

namespace IsuExtra.Entities.People
{
    public class Student : Isu.Entities.Student
    {
        private readonly List<OgNp> _ogNps = new ();

        private Group _studentsGroup;

        public Student(string name, Group group)
        {
            Name = name;
            _studentsGroup = group;
        }

        public void AddOgNp(OgNp ogNp)
        {
            string exceptionInfo = TryFindExceptions(ogNp);

            if (!string.IsNullOrEmpty(exceptionInfo))
                throw new IsuExtraException(exceptionInfo);

            _ogNps.Add(ogNp);
            ogNp.AddStudent(this);
        }

        public bool DoesNotHaveAnyOgNp() =>
            _ogNps.Count == 0;

        public bool HasOgNp(OgNp ogNp) =>
            _ogNps.Contains(ogNp);

        public void RemoveOgNp(string name)
        {
            foreach (OgNp ogNp in _ogNps)
            {
                if (ogNp.Name != name)
                    continue;

                _ogNps.Remove(ogNp);
                return;
            }

            throw new IsuExtraException("No such OgNp");
        }

        public void ChangeGroup(Group newGroup)
        {
            _ogNps.ForEach(u =>
            {
                if (newGroup.Schedule.DoesIntersect(u.Schedule))
                    throw new IsuExtraException("New group schedule intersects with ogNps");
            });

            _studentsGroup = newGroup;
            ChangeGroup(newGroup.Name);
        }

        private string TryFindExceptions(OgNp ogNp)
        {
            if (ogNp.MegaFaculty == _studentsGroup.MegaFaculty)
                return "Same MegaFaculty";

            if (_ogNps.Count >= 2)
                return "Max number of OgNps reached";

            if (ogNp.Schedule.DoesIntersect(_studentsGroup.Schedule))
                return "OgNp schedule intersect with group schedule";

            return DoesIntersectWith_ogNps(ogNp) ?
                "OgNp schedule intersect with another OgNp" :
                null;
        }

        private bool DoesIntersectWith_ogNps(OgNp ogNp) =>
            _ogNps.Count == 1 && _ogNps.First().
                Schedule.DoesIntersect(ogNp.Schedule);
    }
}
