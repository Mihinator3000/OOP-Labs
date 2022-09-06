using System;
using System.Threading.Tasks;
using Refit;
using Reports.Client.Services;
using Reports.Common.Tools;
using Spectre.Console;

namespace Reports.Client
{
    public class ClientService
    {
        public const string HostUrl = "https://localhost:44369";

        private static int? _userId;

        private readonly UserService _userService;
        private readonly TaskService _taskService;
        private readonly ReportService _reportService;
        private readonly AuthService _authService;

        public ClientService()
        {
            _userService = new UserService();
            _taskService = new TaskService();
            _reportService = new ReportService();
            _authService = new AuthService();
        }

        public static int GetUserId() => _userId ?? throw new ReportsAuthException();

        public static void SetUserId(int? userId) => _userId = userId;

        public async Task Start()
        {
            while (true)
            {
                try
                {
                    string line = Console.ReadLine();

                    if (string.IsNullOrEmpty(line))
                        return;

                    string[] input = line.Split(' ');

                    switch (input[0])
                    {
                        case "/e":
                        case "/exit":
                            return;

                        case "/login":
                            await _authService.Login();
                            Console.WriteLine();
                            continue;

                        case "/logout":
                            AuthService.LogOut();
                            continue;

                        default:
                            if (input.Length > 1)
                                break;
                            
                            AnsiConsole.MarkupLine("[red]Incorrect command[/]");
                            Console.WriteLine();
                            continue;
                    }

                    string command = $"{input[0]} {input[1]}";

                    switch (command)
                    {
                        case "/users print-all":
                            await _userService.PrintAll();
                            break;

                        case "/user print":
                            await _userService.PrintById(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/user create":
                            await _userService.Create();
                            break;

                        case "/user delete":
                            await _userService.Delete(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/user update":
                            await _userService.Update();
                            break;


                        case "/tasks print-all":
                            await _taskService.PrintAll();
                            break;

                        case "/task print":
                            await _taskService.PrintById(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/task for-creation":
                            await _taskService.GetForCreation(input[2]);
                            break;

                        case "/task for-last-change":
                            await _taskService.GetForLastChange(input[2]);
                            break;

                        case "/task user":
                            await _taskService.GetForUser(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/task userchanges":
                            await _taskService.GetForChanges(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/task subordinates":
                            await _taskService.GetForSubordinates(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/task create":
                            await _taskService.Create();
                            break;

                        case "/task update":
                            await _taskService.Update();
                            break;

                        case "/task delete":
                            await _taskService.Delete(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/task comment":
                            await _taskService.AddComment(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/task change-user":
                            await _taskService.ChangeAssignedUser(
                                Convert.ToInt32(input[2]),
                                Convert.ToInt32(input[3]));
                            break;


                        case "/reports print-all":
                            await _reportService.PrintAll();
                            break;

                        case "/report print":
                            await _reportService.PrintById(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/report print-by-user":
                            await _reportService.PrintById(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/report create":
                            await _reportService.Create();
                            break;

                        case "/report add-task":
                            await _reportService.AddTask(
                                Convert.ToInt32(input[2]),
                                Convert.ToInt32(input[3]));
                            break;

                        case "/report update":
                            await _reportService.Update();
                            break;

                        case "/report close":
                            await _reportService.Close(
                                Convert.ToInt32(input[2]));
                            break;

                        case "/report weektasks":
                            await _reportService
                                .GetTasksForTheWeek();
                            break;

                        case "/report subordinates":
                            await _reportService
                                .SubordinatesReports();
                            break;

                        case "/report subordinates-without-report":
                            await _reportService
                                .SubordinatesWithoutReport();
                            break;

                        default:
                            AnsiConsole.MarkupLine("[red]Incorrect command[/]");
                            break;
                    }
                }
                catch (ReportsAuthException)
                {
                    AnsiConsole.MarkupLine("[red]User was not authorized[/]");
                }
                catch (ApiException)
                {
                    AnsiConsole.MarkupLine("[red]Invalid request[/]");
                }
                catch (IndexOutOfRangeException)
                {
                    AnsiConsole.MarkupLine("[red]Incorrect parameters[/]");
                }
                catch (FormatException)
                {
                    AnsiConsole.MarkupLine("[red]Expected int as a parameter[/]");
                }

                Console.WriteLine();
            }
        }
    }
}