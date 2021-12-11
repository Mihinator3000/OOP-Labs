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
    public class UserService : IUserService
    {
        private readonly ReportsContext _context;

        public UserService(ReportsContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        public async Task<List<UserDto>> GetAll()
        {
            return await _context.Users
                .Select(u => u.ToDto())
                .ToListAsync();
        }

        public async Task<UserInfoDto> GetById(int id)
        {
            DbUser dbUser = await GetDbUser(id);
            var user = new UserInfoDto
            {
                User = dbUser.ToDto()
            };

            if (user.LeaderId is not null)
            {
                DbUser dbLeader = await GetDbUser(user.LeaderId.Value);
                user.Leader = dbLeader.ToDto();
            }

            List<UserDto> users = await GetAll();
            user.Subordinates = users.Where(u => u.LeaderId == id).ToList();

            user.UserType = user.LeaderId is null
                ? UserTypes.TeamLeader
                : user.Subordinates.Count == 0
                    ? UserTypes.Employee
                    : UserTypes.Leader;

            return user;
        }

        public async Task Create(UserDto user)
        {
            if (user.LeaderId is not null)
                await GetById(user.LeaderId.Value);

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
            DbUser dbUser = await GetDbUser(id);

            _context.Users.Remove(dbUser);
            _context.Users.ToList().ForEach(u =>
            {
                if (u.LeaderId == id)
                    u.LeaderId = null;
            });

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
            if (user.LeaderId is not null)
            {
                if (user.Id == user.LeaderId.Value)
                    throw new ReportsDbException("Cannot set user as his leader");

                await GetById(user.LeaderId.Value);
            }

            DbUser dbUser = await GetDbUser(user.Id);

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

        internal async Task<DbUser> GetDbUser(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == id)
                            ?? throw new ReportsDbException($"User with id {id} was not found in database");
        }
    }
}