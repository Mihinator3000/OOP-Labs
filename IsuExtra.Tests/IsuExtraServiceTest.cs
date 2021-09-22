using System;
using System.Collections;
using System.Collections.Generic;
using IsuExtra.Entities.Enums;
using IsuExtra.Entities.GroupInfo;
using IsuExtra.Entities.People;
using IsuExtra.Entities.Schedule;
using IsuExtra.Entities.Subjects;
using IsuExtra.Services;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class Test
    {
        private IsuExtraService _isuExtraService;

        [SetUp]
        public void Setup()
        {
            _isuExtraService = new IsuExtraService();
            _isuExtraService.AddGroup(
                new GroupName("M3105"),
                MegaFaculties.Ctm,
                new WeekSchedule(new List<DaySchedule>
                {
                    new (DaysOfTheWeek.Mon)
                    {
                       Schedule = new List<TwoHourClass>
                       {
                           new ("Math", "10:00", "SomeGuy", "213A"),
                           new ("Math", "11:40", "SomeGuy", "213A"),
                           new ("OOP", "15:20", "OtherGuy", "433")
                       }
                    },
                    new (DaysOfTheWeek.Tue)
                    {
                       Schedule = new List<TwoHourClass>
                       {
                           new ("Systems", "8:20", "ThirdGuy", "105"),
                           new ("Systems", "10:00", "ThirdGuy", "105"),
                           new ("ProbTheory", "13:30", "ForthGuy", "433"),
                           new ("ProbTheory", "15:20", "ForthGuy", "433")
                       }
                    },
                    new (DaysOfTheWeek.Wed)
                    {
                       Schedule = new List<TwoHourClass>
                       {
                           new ("English", "6:30", "EnglishGuy", "4012"),
                           new ("English", "8:20", "EnglishGuy", "4012"),
                           new ("English", "10:00", "EnglishGuy", "4012"),
                           new ("English", "11:40", "EnglishGuy", "4012"),
                           new ("English", "13:30", "EnglishGuy", "4012"),
                           new ("English", "15:20", "EnglishGuy", "4012")
                       }
                    },
                    new (DaysOfTheWeek.Thu)
                    {
                       Schedule = new List<TwoHourClass>
                       {
                           new ("Math", "10:00", "SomeGuy", "213A"),
                           new ("Math", "11:40", "SomeGuy", "213A"),
                           new ("ProbTheory", "15:20", "ForthGuy", "433")
                       }
                    },
                    new (DaysOfTheWeek.Fri)
                    {
                       Schedule = new List<TwoHourClass>
                       {
                           new ("Healthy sleep", "4:50", "SomeGuy", "213A"),
                           new ("Healthy sleep", "6:30", "SomeGuy", "213A"),
                           new ("OOP", "15:20", "RandomGuy", "433")
                       }
                    },

                    new (DaysOfTheWeek.Sat)
                    {
                       Schedule = new List<TwoHourClass>
                       {
                           new ("Healthy sleep", "10:00", "SomeGuy", "213A"),
                           new ("Healthy sleep", "11:40", "SomeGuy", "213A"),
                           new ("OOP", "15:20", "OtherGuy", "433")
                       }
                    }
                }));

            _isuExtraService.AddStudent(new GroupName("M3105"), "Mikhail");
        }

        [Test]
        public void AddOgNpToStudent_StudentHasOgNpAndOgNpContainsStudent()
        {
            OgNp ogNp = _isuExtraService.AddOgNp(
                "ComputerSecurity",
                MegaFaculties.Tint,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sat)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("GitLecture", "18:00", "OtherGuy", "3222")
                        }
                    }
                }));

            Student student = _isuExtraService.FindGroup(new GroupName("M3105")).FindStudent("Mikhail");
            student.AddOgNp(ogNp);
            
            Assert.True(student.HasOgNp(ogNp));
            Assert.True(_isuExtraService.FindOgNp("ComputerSecurity").EnrolledStudents().Contains(student));
        }

        [Test]
        public void OgNpRemoveFromStudent_StudentDoesNotHaveAnyOgNp()
        {
            OgNp ogNp = _isuExtraService.AddOgNp(
                "ComputerSecurity",
                MegaFaculties.Tint,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sat)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("GitLecture", "18:00", "OtherGuy", "3222")
                        }
                    }
                }));

            Student student = _isuExtraService.FindGroup(new GroupName("M3105")).FindStudent("Mikhail");
            _isuExtraService.AddOgNp(student.Name, ogNp);

            Assert.IsFalse(student.DoesNotHaveAnyOgNp());
            student.RemoveOgNp("ComputerSecurity");

            Assert.IsTrue(_isuExtraService.FindGroup(new GroupName("M3105")).
                DoNotHaveOgNp().Contains(student));
        }

        [Test]
        public void CannotAddOgNpOfOwnMegaFaculty_ThrowException()
        {
            OgNp ogNp = _isuExtraService.AddOgNp(
                "Programming",
                MegaFaculties.Ctm,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sun)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("Java lecture", "20:00", "CupGuy", "104")
                        }
                    }
                }));

            Assert.Catch<IsuExtraException>(() =>
            {
                _isuExtraService.FindGroup(new GroupName("M3105")).FindStudent("Mikhail").AddOgNp(ogNp);
            });
        }

        [Test]
        public void CannotAddOgNpIntersectsWithSchedule_ThrowException()
        {
            OgNp ogNp = _isuExtraService.AddOgNp(
                "ComputerSecurity",
                MegaFaculties.Tint,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sat)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("GitLecture", "18:00", "OtherGuy", "3222")
                        }
                    },

                    new(DaysOfTheWeek.Wed)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("Security", "10:20", "RandomGuy", "4322")
                        }
                    }
                }));

            Assert.Catch<IsuExtraException>(() =>
            {
                _isuExtraService.FindGroup(new GroupName("M3105")).FindStudent("Mikhail").AddOgNp(ogNp);
            });
        }

        [Test]
        public void CannotAddMoreThanTwoOgNpToStudent_ThrowException()
        {
            OgNp ogNp1 = _isuExtraService.AddOgNp(
                "ComputerSecurity",
                MegaFaculties.Tint,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sat)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("GitLecture", "18:00", "OtherGuy", "3222")
                        }
                    }
                }));

            OgNp ogNp2 = _isuExtraService.AddOgNp(
                "SomeBioTechnology",
                MegaFaculties.Blts,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sun)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("LTS lecture", "19:00", "SomeDude", "43D")
                        }
                    }
                }));

            OgNp ogNp3 = _isuExtraService.AddOgNp(
                "LowTemperatureSystems",
                MegaFaculties.Blts,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sun)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("SMT lecture", "20:30", "SomeDude", "43D")
                        }
                    }
                }));

            Student student = _isuExtraService.FindGroup(new GroupName("M3105")).FindStudent("Mikhail");
            student.AddOgNp(ogNp1);
            student.AddOgNp(ogNp2);

            Assert.Catch<IsuExtraException>(() =>
            {
                student.AddOgNp(ogNp3);
            });
        }

        [Test]
        public void TryAddOgNp_IncorrectTimeFormat_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                _isuExtraService.AddOgNp(
                    "ComputerSecurity",
                    MegaFaculties.Tint,
                    new WeekSchedule(new List<DaySchedule>
                    {
                        new(DaysOfTheWeek.Sat)
                        {
                            Schedule = new List<TwoHourClass>
                            {
                                new("GitLecture", "30:00", "OtherGuy", "3222")
                            }
                        }
                    }));
            });
        }

        [Test]
        public void TryAddOgNpWithSameWeekDays_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                _isuExtraService.AddOgNp(
                    "ComputerSecurity",
                    MegaFaculties.Tint,
                    new WeekSchedule(new List<DaySchedule>
                    {
                        new(DaysOfTheWeek.Sat)
                        {
                            Schedule = new List<TwoHourClass>
                            {
                                new("GitLecture", "10:00", "OtherGuy", "3222")
                            }
                        },

                        new(DaysOfTheWeek.Sat)
                        {
                            Schedule = new List<TwoHourClass>
                            {
                                new("SMT lecture", "20:30", "SomeDude", "43D")
                            }
                        }
                    }));
            });
        }

        [Test]
        public void ChangeGroup_NewGroupScheduleIntersectsWithOgNps_ThrowException()
        {
            OgNp ogNp = _isuExtraService.AddOgNp(
                "ComputerSecurity",
                MegaFaculties.Tint,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sat)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("GitLecture", "18:00", "OtherGuy", "3222")
                        }
                    }
                }));

            Student student = _isuExtraService.FindGroup(new GroupName("M3105")).FindStudent("Mikhail");
            student.AddOgNp(ogNp);

            Group newGroup = _isuExtraService.AddGroup(
                new GroupName("M3105"),
                MegaFaculties.Ctm,
                new WeekSchedule(new List<DaySchedule>
                {
                    new(DaysOfTheWeek.Sat)
                    {
                        Schedule = new List<TwoHourClass>
                        {
                            new("IntersectedLecture", "19:20", "SomeCTMDude", "432")
                        }
                    }
                }));
            
            Assert.Catch<IsuExtraException>((() =>
            {
                _isuExtraService.ChangeStudentGroup(student, newGroup);
            }));
        }
    }
}
