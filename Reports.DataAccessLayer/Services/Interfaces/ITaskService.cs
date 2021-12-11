using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Common.DataTransferObjects;

namespace Reports.DataAccessLayer.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAll();

        Task<TaskDto> GetById(int id);
        
        Task<List<TaskDto>> GetForCreation(DateTime time);
        Task<List<TaskDto>> GetForLastChange(DateTime time);

        Task<List<TaskDto>> GetForUser(int userId);
        Task<List<TaskDto>> GetForUserChanges(int userId);
        Task<List<TaskDto>> GetForSubordinates(int userId);

        Task Create(TaskDto task);

        Task Update(TaskDto task, int userId);

        Task Delete(int id);

        Task AddComment(int id, int userId, CommentDto comment);

        Task ChangeAssignedUser(int id, int userId);
    }
}