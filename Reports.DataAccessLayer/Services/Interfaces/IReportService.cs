using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Common.DataTransferObjects;

namespace Reports.DataAccessLayer.Services.Interfaces
{
    public interface IReportService
    { 
        Task<List<ReportDto>> GetAll();

        Task<List<ReportDto>> GetByUserId(int userId);

        Task<ReportDto> GetById(int id);

        Task Create(ReportDto report, int userId);

        Task AddTask(int id, int taskId);

        Task Update(ReportDto report);

        Task Close(int id);

        Task<List<TaskDto>> GetTasksForTheWeek();

        Task<List<ReportDto>> SubordinatesReports(int userId);

        Task<List<UserDto>> SubordinatesWithoutReport(int userId);
    }
}