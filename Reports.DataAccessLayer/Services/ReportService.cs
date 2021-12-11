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
    public class ReportService : IReportService
    {
        private readonly ReportsContext _context;

        public ReportService(ReportsContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        public async Task<List<ReportDto>> GetAll()
        {
            return await _context.Reports
                .Include(u => u.User)
                .Include(u => u.Tasks)
                    .ThenInclude(u => u.AssignedUser)
                .Select(u => u.ToDto())
                .ToListAsync();
        }

        public async Task<List<ReportDto>> GetByUserId(int userId)
        {
            return (await GetDbReportsByUserId(userId))
                .Select(u => u.ToDto())
                .ToList();
        }

        public async Task<ReportDto> GetById(int id)
        {
            return (await GetDbReport(id)).ToDto();
        }

        public async Task Create(ReportDto report, int userId)
        {
            DbUser user = await new UserService(_context).GetDbUser(userId);

            var dbReport = DbReport.FromDto(report);
            dbReport.State = ReportStates.Open;
            dbReport.User = user;

            await _context.Reports.AddAsync(dbReport);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Task was not added to database");
            }

            if (report.Tasks is null)
                return;

            foreach (TaskDto tasks in report.Tasks)
            {
                await AddTask(report.Id, tasks.Id);
            }
        }

        public async Task AddTask(int id, int taskId)
        {
            DbReport dbReport = await GetDbReport(id);
            dbReport.Tasks.AddRange(
                _context.Tasks
                    .Where(u => u.Id == taskId)
                    .ToList());

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Failed to add task to report");
            }
        }

        public async Task Update(ReportDto report)
        {
            DbReport dbReport = await GetDbReport(report.Id);
            dbReport.Update(report);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Task was not updated");
            }
        }

        public async Task Close(int id)
        {
            DbReport report = await GetDbReport(id);

            report.State = ReportStates.Closed;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ReportsDbException("Report was not closed");
            }
        }

        public async Task<List<TaskDto>> GetTasksForTheWeek()
        {
            DateTime weekStartTime = DateTime.Now - TimeSpan.FromDays(7);

            return await _context.Tasks
                .Where(u => u.CreationTime > weekStartTime)
                .Select(u => u.ToDto())
                .ToListAsync();
        }

        public async Task<List<ReportDto>> SubordinatesReports(int userId)
        {
            List<UserDto> users = await GetSubordinates(userId);

            var subordinatesReports = new List<ReportDto>();

            foreach (UserDto user in users)
            {
                List<DbReport> reports = await GetDbReportsByUserId(user.Id);
                if (reports is null)
                    continue;
                
                subordinatesReports.AddRange(reports
                    .Where(u => u.State == ReportStates.Open)
                    .Select(u => u.ToDto()));
            }

            return subordinatesReports;
        }

        public async Task<List<UserDto>> SubordinatesWithoutReport(int userId)
        {
            List<UserDto> users = await GetSubordinates(userId);

            var subordinatesWithoutReports = new List<UserDto>();

            foreach (UserDto user in users)
            {
                List<DbReport> reports = await GetDbReportsByUserId(user.Id);
                if (reports is null || reports.All(u => u.State == ReportStates.Open)) 
                    subordinatesWithoutReports.Add(user);
            }

            return subordinatesWithoutReports;
        }

        private async Task<List<UserDto>> GetSubordinates(int userid)
        {
            return await _context.Users
                .Where(u => u.LeaderId == userid)
                .Select(u => u.ToDto())
                .ToListAsync();
        }
        
        private async Task<DbReport> GetDbReport(int id)
        {
            return await _context.Reports
                        .Include(u => u.User)
                        .Include(u => u.Tasks)
                            .ThenInclude(u => u.AssignedUser)
                        .SingleOrDefaultAsync(u => u.Id == id)
                   ?? throw new ReportsDbException($"Report with id {id} was not found in database");
        }

        private async Task<List<DbReport>> GetDbReportsByUserId(int userId)
        {
            return await _context.Reports
                    .Include(u => u.User)
                    .Include(u => u.Tasks)
                        .ThenInclude(u => u.AssignedUser)
                    .Where(u => u.User.Id == userId)
                    .ToListAsync();
        }
    }
}