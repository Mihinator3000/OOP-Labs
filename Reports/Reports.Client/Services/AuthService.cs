using System.Threading.Tasks;
using Refit;
using Reports.Client.Clients;
using Reports.Common.DataTransferObjects;
using Spectre.Console;

namespace Reports.Client.Services
{
    public class AuthService
    {
        private readonly IAuthClient _authClient;
        
        public AuthService()
        {
            _authClient = RestService.For<IAuthClient>(ClientService.HostUrl);
        }

        public async Task<int> GetId(LoginDto info)
        {
            return await _authClient.GetId(info);
        }

        public async Task CreateAccount(int userId)
        {
            while (true)
            {
                try
                {
                    var info = new LoginDto
                    {
                        Login = AnsiConsole.Ask<string>("Enter [lightgreen]login[/]:"),
                        Password = AnsiConsole.Prompt(new TextPrompt<string>("Enter [green]password:[/]").Secret()),
                        UserId = userId
                    };

                    await _authClient.CreateAccount(info);
                    return;
                }
                catch (ApiException)
                {
                    AnsiConsole.MarkupLine("[red]Account with that login already exists[/]");
                }
            }
        }

        public async Task DeleteAccount(int userId)
        {
            await _authClient.DeleteAccount(userId);
        }

        public async Task Login()
        {
            var info = new LoginDto
            {
                Login = AnsiConsole.Ask<string>("Enter [lightgreen]login[/]:"),
                Password = AnsiConsole.Prompt(new TextPrompt<string>("Enter [green]password:[/]").Secret()),
            };

            try
            {
                ClientService.SetUserId(await _authClient.GetId(info));
            }
            catch (ApiException)
            {
                AnsiConsole.MarkupLine("[red]Login or password is incorrect[/]");
            }
        }

        public static void LogOut()
        {
            ClientService.GetUserId();

            ClientService.SetUserId(null);
        }
    }
}