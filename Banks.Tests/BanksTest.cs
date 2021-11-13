using System;
using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Clients.Passport;
using Banks.Enums;
using Banks.Services.Time;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    public class Tests
    {
        private IMainBank _mainBank;

        private const decimal MaxSummForDubiousClients = 1000;
        private const decimal CreditComission = 5;
        private const decimal CreditLimit = -500;
        private const decimal DebitInterest = 0.5M;

        private const int DayShift = 15;

        [SetUp]
        public void Setup()
        {
            _mainBank = new MainBank();
            _mainBank.RegisterBank(
                new BankBuilder()
                    .SetMaxSummForDubiousClients(MaxSummForDubiousClients)
                    .SetCreditComission(CreditComission)
                    .SetCreditLimit(CreditLimit)
                    .SetDebitInterest(DebitInterest)
                    .Build());

        }

        [Test]
        public void TransactionBetweenAccountsMoneyTransacted()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 1000 });

            const decimal openingBalance1 = 30;
            const decimal openingBalance2 = 40;
            const decimal transactionAmount = 15.5M;

            AbstractAccount account1 = bank.AddAccount(client, AccountTypes.Credit, openingBalance1);
            AbstractAccount account2 = bank.AddAccount(client, AccountTypes.Debit, openingBalance2);

            bank.Transact(account1, account2, transactionAmount);

            Assert.That(account1.Balance is openingBalance1 - transactionAmount);
            Assert.That(account2.Balance is openingBalance2 + transactionAmount);
        }

        [Test]
        public void TransactionCancelledMoneyReturned()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 1000 });

            const decimal openingBalance1 = 30;
            const decimal openingBalance2 = 40;
            const decimal transactionAmount = 15.5M;

            AbstractAccount account1 = bank.AddAccount(client, AccountTypes.Credit, openingBalance1);
            AbstractAccount account2 = bank.AddAccount(client, AccountTypes.Debit, openingBalance2);

            bank.Transact(account1, account2, transactionAmount);
            bank.Transactions[0].Cancel();

            Assert.That(account1.Balance is openingBalance1);
            Assert.That(account2.Balance is openingBalance2);
        }

        [Test]
        public void WithdrawalReplenishmentMoneyChanged()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 1000 });

            const decimal openingBalance = 10;
            const decimal replenishmentAmount = 30;
            const decimal withdrawalAmount = 40;

            AbstractAccount account = bank.AddAccount(client, AccountTypes.Debit, openingBalance);

            bank.Replenish(account, replenishmentAmount);
            bank.Withdraw(account, withdrawalAmount);

            Assert.That(account.Balance is openingBalance + replenishmentAmount - withdrawalAmount);
        }

        [Test]
        public void CreditAccountComissionWithNegativeBalance()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 1000 });

            const decimal openingBalance = 10;
            const decimal withdrawalAmount = 50;
            
            AbstractAccount account = bank.AddAccount(client, AccountTypes.Credit, openingBalance);

            bank.Withdraw(account, withdrawalAmount);

            GlobalTime.AddTime(TimeSpan.FromDays(DayShift));

            _mainBank.AccruePercentage();

            Assert.That(account.Balance is openingBalance - withdrawalAmount - CreditComission * DayShift);
        }

        [Test]
        public void DebitAccountInterestAccuration()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 1000 });

            const decimal openingBalance = 10;

            AbstractAccount account = bank.AddAccount(client, AccountTypes.Debit, openingBalance);

            GlobalTime.AddTime(TimeSpan.FromDays(DayShift));

            _mainBank.AccruePercentage();

            const decimal finalBalance = openingBalance + openingBalance * DebitInterest * DayShift / 365;
            Assert.That(account.Balance is finalBalance);
        }

        [Test]
        public void DepositAccountInterestAccuration()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 1000 });

            const decimal openingBalance = 10;
            const decimal depositInterest = 3;

            AbstractAccount account = bank.AddAccount(client, AccountTypes.Deposit, openingBalance);

            GlobalTime.AddTime(TimeSpan.FromDays(DayShift));

            _mainBank.AccruePercentage();

            const decimal finalBalance = openingBalance + openingBalance * depositInterest * DayShift / 365;
            Assert.That(account.Balance is finalBalance);
        }

        [Test]
        public void CreditLimitChangedClientNotified()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 1000 });

            AbstractAccount account = bank.AddAccount(client, AccountTypes.Credit);
            account.NotifyClient = true;

            bank.CreditLimit = -200;

            Assert.That(client.Notifications.Count is 1);
        }

        [Test]
        public void ClientIsUntrustWorthyCannotDoTheWithdrawal_ThrowException_AfterAddingInfo_DoesNotThrow()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 2000 });

            const decimal openingBalance = 1500;
            const decimal withdrawalAmount = 1200;

            AbstractAccount account = bank.AddAccount(client, AccountTypes.Debit, openingBalance);

            Assert.Catch<BanksException>(() =>
            {
                bank.Withdraw(account, withdrawalAmount);
            });

            client.Address = "Some address";
            client.Passport = new PassportInfo("4444 555555");

            Assert.DoesNotThrow(() =>
            {
                bank.Withdraw(account, withdrawalAmount);
            });
        }

        [Test]
        public void CannotWithdrawFromDepositAccount_ThrowException()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 2000 });

            AbstractAccount account = bank.AddAccount(client, AccountTypes.Deposit, 20);

            Assert.Catch<BanksException>(() =>
            {
                bank.Withdraw(account, 10);
            });
        }

        [Test]
        public void CannotHaveNegativeBalanceOnDebit_ThrowException()
        {
            IBank bank = _mainBank.GetBank(0);

            IClient client = _mainBank.AddClient(
                new Client("Mikhail", "Koshkin") { Balance = 2000 });

            const decimal openingBalance = 200;
            const decimal withdrawalAmount = 300;

            AbstractAccount account = bank.AddAccount(client, AccountTypes.Debit, openingBalance);

            Assert.Catch<BanksException>(() =>
            {
                bank.Withdraw(account, withdrawalAmount);
            });
        }
    }
}