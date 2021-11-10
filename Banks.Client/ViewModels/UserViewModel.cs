using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Banks.Client.Commands;
using Banks.Client.Storages;
using Banks.Client.Views;
using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Clients.Passport;
using Banks.Enums;
using Banks.Tools;

namespace Banks.Client.ViewModels
{
    public class UserViewModel : ViewModel
    {
        private readonly IClient _client;

        private string _notificationButtonContent;
        private decimal _openingBalance;

        public UserViewModel()
            : this(new Entities.Clients.Client(null, null))
        {
        }

        public UserViewModel(IClient client)
        {
            _client = client;
            Storage.GetInstance
                .Bank
                .FindAccounts(client)
                .ForEach(u =>
                    Accounts.Add(new AccountViewModel(u)));

            SetNotificationButtonContent();

            IncreaseBalance = new BaseCommand(OnIncreaseBalance);
            ShowNotifications = new BaseCommand(OnShowNotification);

            OpenAccount = new BaseCommand(OnOpenAccount);

            Replenish = new BaseCommand(OnReplenish);
            Withdraw = new BaseCommand(OnWithdraw);
            Transact = new BaseCommand(OnTransact);

            SelectedAccountType = AccountTypes[0];
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            new AuthenticationWindow().Show();
        }

        public ObservableCollection<AccountViewModel> Accounts { get; } = new ();

        public string FullName =>
            $"{_client.Name} {_client.Surname}";

        public decimal Balance
        {
            get => decimal.Round(_client.Balance, 3);
            set
            {
                try
                {
                    _client.Balance = value;
                    OnPropertyChanged();
                }
                catch (BanksException)
                {
                    MessageBox.Show("Balance of client cannot be less than zero");
                }
            }
        }

        public string NotificationButtonContent
        {
            get => _notificationButtonContent;
            set
            {
                _notificationButtonContent = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _client.Address;
            set
            {
                _client.Address = value == string.Empty
                    ? null
                    : value;
                OnPropertyChanged();
            }
        }

        public string Passport
        {
            get
            {
                PassportInfo passport = _client.Passport;
                return passport is null
                    ? string.Empty
                    : $"{passport.Batch} {passport.Code}";
            }
            set => _client.Passport = string.IsNullOrEmpty(value)
                ? null
                : new PassportInfo(value);
        }

        public decimal OpeningBalance
        {
            get => _openingBalance;
            set
            {
                _openingBalance = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> AccountTypes { get; } =
            new()
            {
                "Credit",
                "Debit",
                "Deposit"
            };

        public string SelectedAccountType { get; set; }


        public ICommand IncreaseBalance { get; }
        public ICommand ShowNotifications { get; }

        public ICommand OpenAccount { get; }

        public ICommand Replenish { get; }
        public ICommand Withdraw { get; }
        public ICommand Transact { get; }

        private void OnIncreaseBalance(object obj)
        {
            Balance += 100;
        }

        private void OnShowNotification(object obj)
        {
            if (_client.Notifications.Count == 0)
                return;

            var stringBuilder = new StringBuilder();

            foreach (string clientNotification in _client.Notifications)
                stringBuilder.AppendLine(clientNotification);

            MessageBox.Show(stringBuilder.ToString());

            _client.ClearNotifications();
            SetNotificationButtonContent();
        }

        private void OnOpenAccount(object obj)
        {
            try
            {
                Enum.TryParse(SelectedAccountType, out AccountTypes accountType);

                IBank bank = Storage.GetInstance.Bank;
                bank.AddAccount(_client, accountType, OpeningBalance);
                Accounts.Add(new AccountViewModel(
                    bank.FindAccounts(_client).Last()));
                OnPropertyChanged(nameof(Balance));
                OpeningBalance = 0;
            }
            catch (BanksException e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
        }

        private void OnReplenish(object obj)
        {
            ExecuteAction(Storage.GetInstance.Bank.Replenish, obj);
        }

        private void OnWithdraw(object obj)
        {
            ExecuteAction(Storage.GetInstance.Bank.Withdraw, obj);
        }

        private void ExecuteAction(Action<AbstractAccount, decimal> action, object obj)
        {
            try
            {
                var account = (AbstractAccount)obj;
                AccountViewModel accountViewModel = GetAccountViewModel(account);
                action.Invoke(account,
                    accountViewModel.TransactionAmount);

                OnPropertyChanged(nameof(Balance));
                accountViewModel.NotifyBalanceChaned();
                accountViewModel.TransactionAmount = 0;
            }
            catch (BanksException e)
            {
                MessageBox.Show($"OperationFailes: {e.Message}");
            }
        }

        private void OnTransact(object obj)
        {
            try
            {
                var account = (AbstractAccount)obj;
                AccountViewModel accountViewModel = GetAccountViewModel(account);

                string receiverId = accountViewModel.ReceiverId;

                IMainBank mainBank = Storage.GetInstance.MainBank;
                IBank bankWithReceiverAccount = mainBank.Banks.FirstOrDefault(u =>
                    u.Accounts.FirstOrDefault(a => a.Id.ToString() == receiverId) != null);

                if (bankWithReceiverAccount is null)
                {
                    MessageBox.Show($"No account with id {receiverId} found");
                    return;
                }

                AbstractAccount receiverAccount = bankWithReceiverAccount
                    .Accounts
                    .First(u => u.Id.ToString() == receiverId);

                Storage.GetInstance.Bank.Transact(
                    account,
                    receiverAccount,
                    accountViewModel.TransactionAmount);

                OnPropertyChanged(nameof(Balance));
                foreach (AccountViewModel accountVm in Accounts)
                {
                    accountVm.NotifyBalanceChaned();
                }
                accountViewModel.TransactionAmount = 0;
                accountViewModel.ReceiverId = string.Empty;
            }
            catch (BanksException e)
            {
                MessageBox.Show($"OperationFailes: {e.Message}");
            }
        }

        private AccountViewModel GetAccountViewModel(AbstractAccount account)
        {
            return Accounts
                .FirstOrDefault(u =>
                    u.Account == account);
        }

        private void SetNotificationButtonContent()
        {
            NotificationButtonContent =
                _client.Notifications.Count == 0
                    ? "No notifications"
                    : $"Show notifications ({_client.Notifications.Count})";
        }
    }
}
