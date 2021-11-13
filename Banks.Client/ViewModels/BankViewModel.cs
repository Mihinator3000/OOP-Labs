using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Banks.Client.Commands;
using Banks.Client.Storages;
using Banks.Client.Views;
using Banks.Entities.Banks;
using Banks.Entities.Transactions;
using Banks.Services.Time;
using Banks.Tools;

namespace Banks.Client.ViewModels
{
    public class BankViewModel : ViewModel
    {
        private readonly IBank _bank;

        private decimal _maxSummForDubiousClients;
        private decimal _creditComission;
        private decimal _creditLimit;
        private decimal _debitInterest;

        public BankViewModel()
        {
            _bank = Storage.GetInstance.Bank;

            _bank.Transactions.ForEach(u =>
                Transactions.Add(new TransactionViewModel(u)));

            _maxSummForDubiousClients = _bank.MaxSummForDubiousClients;
            _creditComission = _bank.CreditComission;
            _creditLimit = _bank.CreditLimit;
            _debitInterest = _bank.DebitInterest;

            ApplyChanges = new BaseCommand(OnApplyChages);
            CancelTransaction = new BaseCommand(OnCancelTransaction);
            AddDay = new BaseCommand(OnAddDay);
            AccruePercentage = new BaseCommand(OnAccruePercentage);

            var timer = new Timer { Interval = 1000};
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            new AuthenticationWindow().Show();
        }

        public ObservableCollection<TransactionViewModel> Transactions { get; } = new ();

        public decimal MaxSummForDubiousClients
        {
            get => _maxSummForDubiousClients;
            set
            {
                _maxSummForDubiousClients = value;
                OnPropertyChanged();
            }
        }

        public decimal CreditComission
        {
            get => _creditComission;
            set
            {
                _creditComission = value;
                OnPropertyChanged();
            }
        }

        public decimal CreditLimit
        {
            get => _creditLimit;
            set
            {
                _creditLimit = value;
                OnPropertyChanged();
            }
        }

        public decimal DebitInterest
        {
            get => _debitInterest;
            set
            {
                _debitInterest = value;
                OnPropertyChanged();
            }
        }

        public DateTime Time =>
            GlobalTime.Now;

        public ICommand ApplyChanges { get; }

        public ICommand CancelTransaction { get; }

        public ICommand AddDay { get; }

        public ICommand AccruePercentage { get; }

        private void OnApplyChages(object obj)
        {
            if (_bank.MaxSummForDubiousClients != _maxSummForDubiousClients)
                _bank.MaxSummForDubiousClients = _maxSummForDubiousClients;

            if (_bank.CreditComission != _creditComission)
                _bank.CreditComission = _creditComission;

            if (_bank.CreditLimit != _creditLimit)
                _bank.CreditLimit = _creditLimit;

            if (_bank.DebitInterest != _debitInterest)
                _bank.DebitInterest = _debitInterest;
        }

        private void OnCancelTransaction(object obj)
        {
            try
            {
                var transaction = (ITransaction)obj;
                transaction.Cancel();

                Transactions
                    .First(u =>
                        u.TransactionInfo == transaction)
                    .NotifyCancelledChanged();
            }
            catch (BanksException e)
            {
                MessageBox.Show($"An error occured: {e.Message}");
            }
        }

        private void OnAddDay(object obj)
        {
            GlobalTime.AddTime(TimeSpan.FromDays(1));
            OnPropertyChanged(nameof(Time));
        }

        private static void OnAccruePercentage(object obj)
        {
            Storage.GetInstance.MainBank.AccruePercentage();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnPropertyChanged(nameof(Time));
        }
    }
}