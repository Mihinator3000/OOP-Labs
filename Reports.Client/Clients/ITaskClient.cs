using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Reports.Common.DataTransferObjects;

namespace Reports.Client.Clients
{
    public interface ITaskClient
    {
        [Get("/api/task/get/all")]
        Task<List<TaskDto>> GetAll();

        [Get("/api/task/get/{id}")]
        Task<TaskDto> GetById(int id);

        [Get("/api/task/get/creation")]
        Task<List<TaskDto>> GetForCreation([Body] DateTimeDto time);

        [Get("/api/task/get/lastChange")]
        Task<List<TaskDto>> GetForLastChange([Body] DateTimeDto time);

        [Get("/api/task/get/user/{userId}")]
        Task<List<TaskDto>> GetForUser(int userId);

        [Get("/api/task/get/userChanges/{userId}")]
        Task<List<TaskDto>> GetForUserChanges(int userId);

        [Get("/api/task/get/userSubordinates/{userId}")]
        Task<List<TaskDto>> GetForSubordinates(int userId);

        [Post("/api/task/create")]
        Task Create([Body] TaskDto task);

        [Post("/api/task/update/user/{userId}")]
        Task Update([Body] TaskDto task, int userId);

        [Post("/api/task/delete/{id}")]
        Task Delete(int id);
        
        [Post("/api/task/comment/{id}/user/{userId}")]
        Task AddComment([Body] CommentDto comment, int id, int userId);

        [Post("/api/task/assign/{id}/user/{userId}")]
        Task ChangeAssignedUser(int id, int userId);
    }
}