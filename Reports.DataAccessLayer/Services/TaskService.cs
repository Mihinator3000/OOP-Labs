using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Common.DataTransferObjects;
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

        public async Task<List<UserDto>> GetAll()
        {
            return await _context.Users.Select(u => u.ToDto()).ToListAsync();
        }

        public async Task<UserDto> GetById(int id)
        {
            DbUser dbUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == id)
                            ?? throw new ReportsDbException($"User with id {id} was not found in database");

            return dbUser.ToDto();
        }

        public async Task Create(UserDto user)
        {
            await _context.Users.AddAsync(DbUser.FromDto(user));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("User was not added to database");
            }
        }

        public async Task Delete(int id)
        {
            DbUser dbUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new ReportsDbException($"User with id {id} was not found in database");

            _context.Users.Remove(dbUser);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("User was not deleted from database");
            }
        }

        public async Task Update(UserDto user)
        {
            DbUser dbUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

            dbUser.Update(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("User was not updated");
            }
        }
        /*- GetAll
- поиск задач по ID
- поиск задач по времени создания/последнего изменения
- поиск задач, закреплённых за определённым пользователем(сотрудником команды)
- поиск задач, в которые пользователь вносил изменения
- создание задачи, изменение её состояния, добавления к ней комментария, изменение назначенного за ней сотрудника
- получение списка задач, которые назначены подчинённым определённого сотрудника
- Добавление задачи, обновление описания задачи
- Изменение человека, который заасайнен*/
    }
}