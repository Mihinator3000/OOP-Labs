using IsuExtra.Entities.Enums;
using IsuExtra.Entities.Schedule;

namespace IsuExtra.Entities.GroupInfo
{
    public class OgNp : BaseGroup
    {
        public OgNp(string name, MegaFaculties megaFaculties, WeekSchedule weekSchedule)
            : base(null, megaFaculties, weekSchedule)
        {
            Name = name;
        }

        public new string Name { get; }
    }
}