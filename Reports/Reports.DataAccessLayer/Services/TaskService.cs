using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Common.DataTransferObjects;
using Reports.Common.Enums;
using Reports.Common.Tools;
using Reports.DataAccessLayer.Entities;
using Reports.DataAccessLayer.Services.Interfaces;

namespace Reports.DataAccessLayer.Services
{
    public class TaskService : ITaskService
    {
        private readonly ReportsContext _context;

        public TaskService(ReportsContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        public async Task<List<TaskDto>> GetAll()
        {
            return await GetDbTasks()
                .Select(u => u.ToDto())
                .ToListAsync();
        }

        public async Task<TaskDto> GetById(int id)
        {
            DbTask dbTask = await GetDbTask(id);
            return dbTask.ToDto();
        }

        public async Task<List<TaskDto>> GetForCreation(DateTime time)
        {
            List<TaskDto> tasks = await GetAll();
            return tasks
                .Where(u => u.CreationTime.Date == time.Date)
                .ToList();
        }

        public async Task<List<TaskDto>> GetForLastChange(DateTime time)
        {
            List<TaskDto> tasks = await GetAll();
            return tasks
                .Where(u => u.Changes
                    .Last().Time.Date == time.Date)
                .ToList();
        }

        public async Task<List<TaskDto>> GetForUser(int userId)
        {
            List<TaskDto> tasks = await GetAll();
            return tasks
                .Where(u => u.AssignedUser is not null
                            && u.AssignedUser.Id == userId)
                .ToList();
        }

        public async Task<List<TaskDto>> GetForUserChanges(int userId)
        {
            List<TaskDto> tasks = await GetAll();
            return tasks
                .Where(u => u.Changes
                    .Any(c => c.User.Id == userId))
                .ToList();
        }

        public async Task<List<TaskDto>> GetForSubordinates(int userId)
        {
            UserInfoDto user = await new UserService(_context).GetById(userId);

            var tasks = new List<TaskDto>();
            foreach (UserDto subordinate in user.Subordinates)
            {
                tasks.AddRange(await GetForUser(subordinate.Id));
            }

            return tasks;
        }

        public async Task Create(TaskDto task)
        {
            var dbTask = DbTask.FromDto(task);
            dbTask.CreationTime = DateTime.Now;
            dbTask.State = TaskStates.Open;

            await _context.Tasks.AddAsync(dbTask);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Task was not added to database");
            }
        }

        public async Task Update(TaskDto task, int userId)
        {
            DbTask dbTask = await GetDbTask(task.Id);

            DbUser dbUser = await new UserService(_context).GetDbUser(userId);

            if (dbTask.Description != task.Description)
            {
                dbTask.Changes.Add(new DbChange
                {
                    Time = DateTime.Now,
                    ChangeType = TaskChangeTypes.Description,
                    Message = task.Description,
                    User = dbUser
                });
            }

            if (dbTask.State != task.State)
            {
                dbTask.Changes.Add(new DbChange
                {
                    Time = DateTime.Now,
                    ChangeType = TaskChangeTypes.State,
                    Message = task.State.ToString(),
                    User = dbUser
                });
            }

            dbTask.Update(task);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("User was not updated");
            }
        }

        public async Task Delete(int id)
        {
            DbTask dbTask = await GetDbTask(id);

            _context.Tasks.Remove(dbTask);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Task was not deleted from database");
            }
        }

        public async Task AddComment(int id, int userId, CommentDto comment)
        {
            DbTask task = await GetDbTask(id);
            task.Comments.Add(DbComment.FromDto(comment));
            task.Changes.Add(new DbChange
            {
                Time = DateTime.Now,
                ChangeType = TaskChangeTypes.Comment,
                Message = comment.Commentary,
                User = await new UserService(_context).GetDbUser(userId)
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Comment for task was not created");
            }
        }

        public async Task ChangeAssignedUser(int id, int userId)
        {
            DbUser dbUser = await new UserService(_context).GetDbUser(userId);

            DbTask task = await GetDbTask(id);

            if (task.AssignedUser is not null)
            {
                if (task.AssignedUser.Id == userId)
                    throw new ReportsDbException("User already assigned to the task");

                task.Changes.Add(new DbChange
                {
                    Time = DateTime.Now,
                    ChangeType = TaskChangeTypes.AssignedUser,
                    Message = userId.ToString(),
                    User = dbUser
                });
            }

            task.AssignedUser = dbUser;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Assigned user was not updated");
            }
        }

        private async Task<DbTask> GetDbTask(int id)
        {
            return await GetDbTasks()
                       .SingleOrDefaultAsync(u => u.Id == id) 
                   ?? throw new ReportsDbException($"Task with id {id} was not found in database");
        }

        private IQueryable<DbTask> GetDbTasks()
        {
            return _context.Tasks
                .Include(u => u.AssignedUser)
                .Include(u => u.Changes)
                    .ThenInclude(u => u.User)
                .Include(u => u.Comments);
        }
    }
}