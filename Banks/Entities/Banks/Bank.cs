using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Accounts;
using Banks.Entities.Accounts.Balance;
using Banks.Entities.Accounts.Creators;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Enums;
using Banks.Services.Time;
using Banks.Tools;

namespace Banks.Entities.Banks
{
    public class Bank : IBank
    {
        private DateTime _previousAccrualTime;

        private decimal _maxSummForDubiousClients;
        private decimal _creditComission;
        private decimal _creditLimit;
        private decimal _debitInterest;

        internal Bank()
        {
            _previousAccrualTime = GlobalTime.Now;
        }

        public List<AbstractAccount> Accounts { get; } = new ();
        public List<ITransaction> Transactions { get; } = new ();

        public decimal MaxSummForDubiousClients
        {
            get => _maxSummForDubiousClients;
            set
            {
                _maxSummForDubiousClients = value;
                Notify<AbstractAccount>($"Changed max summ for transaction for untrustworthy clients to {value}");
            }
        }

        public decimal CreditComission
        {
            get => _creditComission;
            set
            {
                _creditComission = value;
                Notify<CreditAccount>($"Changed commision to {value}");
            }
        }

        public decimal CreditLimit
        {
            get => _creditLimit;
            set
            {
                _creditLimit = value;
                Notify<CreditAccount>($"Changed credit limit to {value}");
            }
        }

        public decimal DebitInterest
        {
            get => _debitInterest;
            set
            {
                _debitInterest = value;
                Notify<DebitAccount>($"Changed interest to {value}");
            }
        }

        public AbstractAccount AddAccount(IClient client, AccountTypes accountType, decimal balance = 0)
        {
            client.Balance -= balance;

            AbstractAccount account =
                ResolveAccountCreator(accountType)
                .Create(client, balance);

            Accounts.Add(account);
            return account;
        }

        public List<AbstractAccount> FindAccounts(IClient client)
        {
            return Accounts
                .Where(u => u.Client == client)
                .ToList();
        }

        public void DeleteAccount(AbstractAccount account)
        {
            Accounts.Remove(account);
        }

        public void Replenish(AbstractAccount account, decimal amount)
        {
            Transactions.Add(new Replenishment(account, amount).Execute());
        }

        public void Transact(AbstractAccount sender, AbstractAccount receiver, decimal amount)
        {
            CheckIfDubious(sender, amount);
            Transactions.Add(new Transaction(sender, receiver, amount).Execute());
        }

        public void Transact(AbstractAccount sender, Guid receiverId, decimal amount)
        {
            AbstractAccount receiver = Accounts
                .FirstOrDefault(u => u.Id == receiverId)
                ?? throw new BanksException($"Account {receiverId} was not found");

            Transact(sender, receiver, amount);
        }

        public void Withdraw(AbstractAccount account, decimal amount)
        {
            CheckIfDubious(account, amount);
            Transactions.Add(new Withdrawal(account, amount).Execute());
        }

        public void AccrualOfInterestOrCommission(object sender, EventArgs args)
        {
            DateTime currentTime = GlobalTime.Now;

            foreach (AbstractAccount account in Accounts)
            {
                account.History.Add(new BalanceState(account.Balance, currentTime));

                var balanceStates = account.History
                    .Where(u => u.Time > _previousAccrualTime && u.Time <= currentTime)
                    .GroupBy(u => u.Time.Day)
                    .Select(u => u.Last())
                    .ToList();

                for (int i = 1; i < balanceStates.Count; i++)
                {
                    int days = (int)(balanceStates[i].Time - balanceStates[i - 1].Time).TotalDays;

                    account.AccrualOfPercents(balanceStates[i], days);
                }
            }

            _previousAccrualTime = currentTime;
        }

        private IAccountCreator ResolveAccountCreator(AccountTypes accountType)
        {
            return accountType switch
            {
                AccountTypes.Debit =>
                    new DebitAccountCreator(this),
                AccountTypes.Deposit =>
                    new DepositAccountCreator(),
                AccountTypes.Credit =>
                    new CreditAccountCreator(this),
                _ => throw new BanksException(nameof(accountType))
            };
        }

        private void CheckIfDubious(AbstractAccount account, decimal amount)
        {
            if (amount > MaxSummForDubiousClients
                && account.Client.IsDubious())
                throw new BanksException("Client is untrustworthy");
        }

        private void Notify<TAccount>(string notificationMessage)
            where TAccount : AbstractAccount
        {
            Accounts
                .Where(u => u is TAccount)
                .ToList()
                .ForEach(u =>
                {
                    if (u.NotifyClient)
                    {
                        u.Client.AddNotification(notificationMessage);
                    }
                });
        }
    }
}