using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;
using Reports.Client.Clients;
using Reports.Common.DataTransferObjects;
using Reports.Common.Enums;
using Reports.Common.Tools;
using Spectre.Console;

namespace Reports.Client.Services
{
    public class TaskService
    {
        private readonly ITaskClient _taskClient;

        public TaskService()
        {
            _taskClient = RestService.For<ITaskClient>(ClientService.HostUrl);
        }

        public async Task PrintAll()
        {
            PrintTasks((await _taskClient.GetAll()));
        }


        public async Task PrintById(int id)
        {
            PrintTask(await _taskClient.GetById(id));
        }
        
        public async Task GetForCreation(string timeString)
        {
            if (!DateTime.TryParse(timeString, out DateTime time))
                throw new ReportsException("Invalid datatime input");

            PrintTasks(await _taskClient.GetForCreation(time));
        }

        public async Task GetForLastChange(string timeString)
        {
            if (!DateTime.TryParse(timeString, out DateTime time))
                throw new ReportsException("Invalid datatime input");

            PrintTasks(await _taskClient.GetForLastChange(time));
        }

        public async Task GetForUser(int userId)
        {
            PrintTasks(await _taskClient.GetForUser(userId));
        }

        public async Task GetForChanges(int userId)
        {
            PrintTasks(await _taskClient.GetForUserChanges(userId));
        }

        public async Task GetForSubordinates(int userId)
        {
            PrintTasks(await _taskClient.GetForSubordinates(userId));
        }

        public async Task Create()
        {
            var task = new TaskDto
            {
                Name = AnsiConsole.Ask<string>("Enter [lightgreen]task name[/]:"),
                Description = AnsiConsole.Prompt(new TextPrompt<string>("Enter [silver]description[/]:").AllowEmpty())
            };

            await _taskClient.Create(task);
        }

        public async Task Update()
        {
            int userId = ClientService.GetUserId();

            var task = new TaskDto
            {
                Id = Convert.ToInt32(AnsiConsole.Ask<string>("Enter task [blue]id[/]:"))
            };

            string input = AnsiConsole.Prompt(new TextPrompt<string>("Enter new [lightgreen]task name[/]:").AllowEmpty());
            if (input != string.Empty)
                task.Name = input;

            input = AnsiConsole.Prompt(new TextPrompt<string>("Enter new [silver]description[/]:").AllowEmpty());
            if (input != string.Empty)
                task.Description = input;

            input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Enter new [silver]state[/]")
                    .AddChoices("Open", "Active", "Resolved"));

            if (Enum.TryParse(input, out TaskStates taskState))
                task.State = taskState;

            await _taskClient.Update(task, userId);
        }

        public async Task Delete(int id)
        {
            await _taskClient.Delete(id);
        }

        public async Task AddComment(int id)
        {
            int userId = ClientService.GetUserId();

            var comment = new CommentDto
            {
                Commentary = AnsiConsole.Ask<string>("Enter comment:")
            };

            await _taskClient.AddComment(comment, id, userId);
        }

        public async Task ChangeAssignedUser(int id, int userId)
        {
            await _taskClient.ChangeAssignedUser(id, userId);
        }

        internal static void PrintTasks(IEnumerable<TaskDto> tasks)
        {
            foreach (TaskDto task in tasks)
            {
                PrintTask(task);
            }
        }

        private static void PrintTask(TaskDto task)
        {
            AnsiConsole.MarkupLine($"[lightgreen]{task.Name}[/] {task.Id}");
            AnsiConsole.MarkupLine($"[yellow]{task.Description}[/]");
            AnsiConsole.MarkupLine(task.State.ToString());
            if (task.Changes is not null && task.Changes.Any())
            {
                AnsiConsole.MarkupLine($"[purple]Changes:[/]");
                task.Changes.ForEach(u =>
                {
                    AnsiConsole.MarkupLine($"[lightgreen]{u.Time}[/]");
                    AnsiConsole.MarkupLine($"{u.ChangeType} [blue]{u.Message}[/]");
                });
            }
            AnsiConsole.WriteLine();
        }
    }
}