using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Reports.Client.Clients;
using Reports.Common.DataTransferObjects;
using Spectre.Console;

namespace Reports.Client.Services
{
    public class ReportService
    {
        private readonly IReportClient _reportClient;

        public ReportService()
        {
            _reportClient = RestService.For<IReportClient>(ClientService.HostUrl);
        }

        public async Task PrintAll()
        {
            PrintReports(await _reportClient.GetAll());
        }

        public async Task PrintById(int id)
        {
            PrintReport(await _reportClient.GetById(id));
        }

        public async Task PrintByUserId(int userId)
        {
            PrintReports(await _reportClient.GetByUserId(userId));
        }

        public async Task Create()
        {
            int userId = ClientService.GetUserId();
            var report = new ReportDto
            {
                Message = AnsiConsole.Ask<string>("Enter [gray]message[/]:")
            };

            await _reportClient.Create(report, userId);
        }

        public async Task AddTask(int id, int taskId)
        {
            await _reportClient.AddTask(id, taskId);
        }

        public async Task Update()
        {
            var report = new ReportDto
            {
                Id = Convert.ToInt32(AnsiConsole.Ask<string>("Enter report [blue]id[/]:")),
                Message = AnsiConsole.Ask<string>("Enter new [gray]message[/]:")
            };

            await _reportClient.Update(report);
        }

        public async Task Close(int id)
        {
            await _reportClient.CloseReport(id);
        }

        public async Task GetTasksForTheWeek()
        {
            TaskService.PrintTasks(await _reportClient.GetTasksForTheWeek());
        }

        public async Task SubordinatesReports()
        {
            PrintReports(await _reportClient.SubordinatesReports(ClientService.GetUserId()));
        }


        public async Task SubordinatesWithoutReport()
        {
            List<UserDto> users = await _reportClient.SubordinatesWithoutReport(ClientService.GetUserId());
            if (users is null)
                return;

            foreach (UserDto user in users)
            {
                await new UserService().PrintById(user.Id);
            }
        }

        private static void PrintReport(ReportDto report)
        {
            AnsiConsole.MarkupLine(report.Message);
            AnsiConsole.MarkupLine(report.State.ToString());
            AnsiConsole.WriteLine();
        }

        private static void PrintReports(IEnumerable<ReportDto> reports)
        {
            foreach (ReportDto report in reports)
            {
                PrintReport(report);
            }
        }
    }
}