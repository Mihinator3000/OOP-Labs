using Isu.Entities;
using Isu.Entities.GroupInfo;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    [TestFixture]
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
            _isuService.AddGroup(new GroupName("M3105"));
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group group = _isuService.AddGroup(new GroupName("M3106"));
            _isuService.AddStudent(group, "Alice");

            Assert.IsTrue(_isuService.FindGroup(new GroupName("M3106")).
                Contains("Alice"));

            Assert.IsTrue(_isuService.FindStudent("Alice").
                IsInGroup(new GroupName("M3106")));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group group = _isuService.FindGroup(new GroupName("M3105"));
                for (int i = 0; i < 50; i++)
                {
                    _isuService.AddStudent(group, $"Student ¹{i}");
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup(new GroupName("M42052"));
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group group = _isuService.AddGroup(new GroupName("M3106"));
            _isuService.AddStudent(group, "Alice");
            
            Student student = _isuService.FindStudent("Alice");
            _isuService.ChangeStudentGroup(student, 
                _isuService.FindGroup(new GroupName("M3105")));

            Assert.IsFalse(student.IsInGroup(new GroupName("M3106")));
            Assert.IsTrue(student.IsInGroup(new GroupName("M3105")));
        }
    }
}