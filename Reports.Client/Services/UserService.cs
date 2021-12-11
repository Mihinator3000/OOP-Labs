using System;
using System.Linq;
using System.Threading.Tasks;
using Refit;
using Reports.Client.Clients;
using Reports.Common.DataTransferObjects;
using Reports.Common.Enums;
using Spectre.Console;

namespace Reports.Client.Services
{
    public class UserService
    {
        private readonly IUserClient _userClient;

        public UserService()
        {
            _userClient = RestService.For<IUserClient>(ClientService.HostUrl);
        }
        
        public async Task PrintAll()
        {
            var users = (await _userClient.GetAll())
                .Where(u => u.LeaderId is null)
                .ToList();

            foreach (UserDto user in users)
            {
                await PrintTreeFromRoot(user.Id);
            }
        }
        
        public async Task PrintById(int id)
        {
            await PrintTreeFromRoot(id);
        }

        public async Task Create()
        {
            var user = new UserDto
            {
                Name = AnsiConsole.Ask<string>("Enter [lightgreen]username[/]:")
            };


            string input = AnsiConsole.Prompt(new TextPrompt<string>("Enter [bold yellow]leader id[/]:").AllowEmpty());

            user.LeaderId = input == string.Empty ? null : Convert.ToInt32(input);

            await _userClient.Create(user);

            await new AuthService().CreateAccount((await _userClient.GetAll()).Last().Id);
        }

        public async Task Delete(int id)
        {
            await _userClient.Delete(id);
            await new AuthService().DeleteAccount(id);
        }

        public async Task Update()
        {
            var user = new UserDto
            {
                Id = Convert.ToInt32(AnsiConsole.Ask<string>("Enter user [blue]id[/]:")),
                Name = AnsiConsole.Ask<string>("Enter new [lightgreen]username[/]:")
            };

            string input = AnsiConsole.Prompt(new TextPrompt<string>("Enter new [bold yellow]leader id[/]:").AllowEmpty());
            user.LeaderId = input == string.Empty ? null : Convert.ToInt32(input);

            await _userClient.Update(user);
        }

        internal static string UserToString(UserInfoDto user)
        {
            string color = user.UserType switch
            {
                UserTypes.TeamLeader => "bold yellow",
                UserTypes.Leader => "green",
                UserTypes.Employee => "blue",
                _ => throw new ArgumentOutOfRangeException(nameof(user.UserType))
            };

            return $"[{color}]{user.Name}, (Id: {user.Id})[/]";
        }

        private async Task PrintTreeFromRoot(int id)
        {
            UserInfoDto rootUser = await _userClient.GetById(id);
            var tree = new Tree(UserToString(rootUser));
            foreach (UserDto userSubordinate in rootUser.Subordinates)
            {
                UserInfoDto nodeUser = await _userClient.GetById(userSubordinate.Id);

                TreeNode node = tree.AddNode(UserToString(nodeUser));
                await AddNodes(node, nodeUser);
            }

            AnsiConsole.Write(tree);
        }

        private async Task AddNodes(TreeNode mainNode, UserInfoDto user)
        {
            foreach (UserDto userSubordinate in user.Subordinates)
            {
                UserInfoDto nodeUser = await _userClient.GetById(userSubordinate.Id);

                TreeNode node = mainNode.AddNode(UserToString(nodeUser));
                await AddNodes(node, nodeUser);
            }
        }

    }
}