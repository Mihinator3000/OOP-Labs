using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Reports.Common.DataTransferObjects;

namespace Reports.Client.Clients
{
    public interface IReportClient
    {
        [Get("/api/report/get/all")]
        Task<List<ReportDto>> GetAll();
        
        [Get("/api/report/get/user/{userId}")]
        Task<List<ReportDto>> GetByUserId(int userId);

        [Get("/api/report/get/{id}")]
        Task<ReportDto> GetById(int id);
        
        [Post("/api/report/create/user/{userId}")]
        Task Create([Body] ReportDto report, int userId);
        
        [Post("/api/report/addTask/{id}/{taskId}")]
        Task AddTask(int id, int taskId);
        
        [Post("/api/report/update")]
        Task Update(ReportDto report);
        
        [Post("/api/report/close/{id}")]
        Task CloseReport(int id);
        
        [Get("/api/report/get/weektasks")]
        Task<List<TaskDto>> GetTasksForTheWeek();
        
        [Get("/api/report/subordinates/{userId}")]
        Task<List<ReportDto>> SubordinatesReports(int userId);
        
        [Get("/api/report/subordinates/without/{userId}")]
        Task<List<UserDto>> SubordinatesWithoutReport(int userId);
    }
}