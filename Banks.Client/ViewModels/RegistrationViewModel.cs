using System.Windows;
using System.Windows.Input;
using Banks.Client.Actions;
using Banks.Client.Commands;
using Banks.Client.Storages;
using Banks.Entities.Clients;
using Banks.Entities.Clients.Passport;

namespace Banks.Client.ViewModels
{
    public class RegistrationViewModel : ViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Passport { get; set; }

        public ICommand SignUp { get; }

        public RegistrationViewModel()
        {
            SignUp = new BaseCommand(OnSignUp);
        }

        private string Validation()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
                return "Invalid login and password";

            if (LoginStorage.LoginExists(Login))
                return $"Account {Login} already exists";

            if (string.IsNullOrEmpty(Name))
                return "Invalid name input";

            if (string.IsNullOrEmpty(Surname)) 
                return "Invalid surname input";

            return null;
        }

        private void OnSignUp(object obj)
        {
            string validationResult = Validation();
            if (validationResult is not null)
            {
                MessageBox.Show(validationResult);
                return;
            }

            IClient client = new Entities.Clients.Client(Name, Surname);

            if (!string.IsNullOrEmpty(Address))
            {
                client.Address = Address;
            }

            if (!string.IsNullOrEmpty(Passport))
            {
                client.Passport = new PassportInfo(Passport);
            }

            LoginStorage.AddClient($"{Login}||{Password}", client);
            Storage.GetInstance.MainBank.AddClient(client);
            CloseWindows.All("Registration");
        }
    }
}