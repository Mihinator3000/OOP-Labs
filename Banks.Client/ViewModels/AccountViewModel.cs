using System;
using System.Windows.Media;
using Banks.Entities.Accounts;

namespace Banks.Client.ViewModels
{
    public class AccountViewModel : ViewModel
    {
        private decimal _transactionAmount;
        private string _receiverId;

        public AccountViewModel(AbstractAccount account)
        {
            Account = account;
            
            switch (account)
            {
                case CreditAccount:
                    Type = "Credit";
                    Brush = new SolidColorBrush(Colors.DodgerBlue);
                    break;

                case DebitAccount:
                    Type = "Debit";
                    Brush = new SolidColorBrush(Colors.Gold);
                    break;

                case DepositAccount:
                    Type = "Deposit";
                    Brush = new SolidColorBrush(Colors.LimeGreen);
                    Transactable = false;
                    Withrawable = false;
                    break;

                default:
                    throw new ArgumentException(nameof(account));
            }
        }

        public decimal Balance =>
            decimal.Round(Account.Balance, 3);

        public AbstractAccount Account { get; set; }

        public string Type { get; }

        public Brush Brush { get; }

        public bool Transactable { get; } = true;
        public bool Withrawable { get; } = true;

        public decimal TransactionAmount
        {
            get => _transactionAmount;
            set
            {
                _transactionAmount = value;
                OnPropertyChanged();
            }
        }

        public string ReceiverId
        {
            get => _receiverId;
            set
            {
                _receiverId = value;
                OnPropertyChanged();
            }
        }

        public bool Notify
        {
            get => Account.NotifyClient;
            set
            {
                Account.NotifyClient = value;
                OnPropertyChanged();
            }
        }

        public void NotifyBalanceChaned()
        {
            OnPropertyChanged(nameof(Balance));
        }
    }
}