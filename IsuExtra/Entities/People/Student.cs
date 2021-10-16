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
            TryFindExceptions(ogNp);
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

        private void TryFindExceptions(OgNp ogNp)
        {
            if (ogNp.MegaFaculty == _studentsGroup.MegaFaculty)
                throw new IsuExtraException("Same MegaFaculty");

            if (_ogNps.Count >= 2)
                throw new IsuExtraException("Max number of OgNps reached");

            if (ogNp.Schedule.DoesIntersect(_studentsGroup.Schedule))
                throw new IsuExtraException("OgNp schedule intersect with group schedule");

            if (DoesIntersectWith_ogNps(ogNp))
                throw new IsuExtraException("OgNp schedule intersect with another OgNp");
        }

        private bool DoesIntersectWith_ogNps(OgNp ogNp) =>
            _ogNps.Count == 1 && _ogNps.First().
                Schedule.DoesIntersect(ogNp.Schedule);
    }
}
