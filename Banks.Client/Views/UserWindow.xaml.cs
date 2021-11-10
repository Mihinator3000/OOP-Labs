using System.Windows;
using Banks.Client.ViewModels;
using Banks.Entities.Clients;

namespace Banks.Client.Views
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow(IClient client)
        {
            InitializeComponent();
            var userViewModel = new UserViewModel(client);
            DataContext = userViewModel;
            Closing += userViewModel.OnWindowClosing;
        }
    }
}
