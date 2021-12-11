using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Common.DataTransferObjects;
using Reports.Common.Tools;
using Reports.DataAccessLayer.Entities;
using Reports.DataAccessLayer.Services.Interfaces;

namespace Reports.DataAccessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly ReportsContext _context;

        public AuthService(ReportsContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        public async Task<int> GetId(LoginDto info)
        {
            DbLogin dbInfo = await _context.Logins
                .SingleOrDefaultAsync(u =>
                    u.Login == info.Login &&
                    u.Password == info.Password);

            if (dbInfo is null)
                throw new ReportsDbException("Account was not found in database");

            return dbInfo.UserId;
        }

        public async Task CreateAccount(LoginDto info)
        {
            if (await _context.Logins.SingleOrDefaultAsync(u => u.Login == info.Login) is not null)
                throw new ReportsDbException("Account already exists");

            await _context.Logins.AddAsync(DbLogin.FromDto(info));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Account was not added to database");
            }
        }

        public async Task DeleteAccount(int userId)
        {
            DbLogin info = await _context.Logins.SingleOrDefaultAsync(u => u.UserId == userId)
                           ?? throw new ReportsDbException("Account was not found in database");
            
            _context.Logins.Remove(info);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Account was not deleted from database");
            }
        }
    }
}