using System.Windows;
using System.Windows.Input;
using Banks.Client.Actions;
using Banks.Client.Commands;
using Banks.Client.Storages;
using Banks.Client.Views;
using Banks.Entities.Clients;

namespace Banks.Client.ViewModels
{
    public class AuthenticationViewModel : ViewModel
    {
        public AuthenticationViewModel()
        {
            SignIn = new BaseCommand(OnSignIn);
            SignUp = new BaseCommand(OnSignUp);
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public ICommand SignIn { get; }
        public ICommand SignUp { get; }

        private void OnSignIn(object obj)
        {
            string loginPass = $"{Login}||{Password}";
            if (loginPass == "bank||123")
            {
                new BankWindow().Show();
                CloseWindows.All("Authentication");
                return;
            }

            IClient client = LoginStorage.GetClient(loginPass);
            if (client is null)
            {
                MessageBox.Show("Incorrect login or password");
                return;
            }

            new UserWindow(client).Show();
            CloseWindows.All("Authentication");
        }

        private static void OnSignUp(object obj)
        {
            new Registration().Show();
        }
    }
}