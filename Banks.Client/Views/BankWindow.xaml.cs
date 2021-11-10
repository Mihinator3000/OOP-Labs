using System.Windows;
using Banks.Client.ViewModels;

namespace Banks.Client.Views
{
    /// <summary>
    /// Interaction logic for BankWindow.xaml
    /// </summary>
    public partial class BankWindow : Window
    {
        public BankWindow()
        {
            InitializeComponent();
            var userViewModel = new BankViewModel();
            DataContext = userViewModel;
            Closing += userViewModel.OnWindowClosing;
        }
    }
}
