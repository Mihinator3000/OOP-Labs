using System.Collections.Generic;
using System.Linq;
using Reports.Common.DataTransferObjects;
using Reports.Common.Enums;

namespace Reports.DataAccessLayer.Entities
{
    public class DbReport
    {
        public int Id { get; set; }

        public DbUser User { get; set; }

        public string Message { get; set; }

        public ReportStates State { get; set; }

        public List<DbTask> Tasks { get; set; } = new ();

        public static DbReport FromDto(ReportDto report)
        {
            var dbReport = new DbReport();
            dbReport.Update(report);
            return dbReport;
        }

        public void Update(ReportDto report)
        {
            Id = report.Id;
            Message = report.Message;
            State = report.State;
        }

        public ReportDto ToDto()
        {
            return new ReportDto
            {
                Id = Id,
                User = User.ToDto(),
                Message = Message,
                State = State,
                Tasks = Tasks
                    .Select(u => u.ToDto())
                    .ToList()
            };
        }
    }
}