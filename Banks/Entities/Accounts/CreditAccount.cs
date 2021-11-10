using System;
using Banks.Entities.Accounts.Balance;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Tools;

namespace Banks.Entities.Accounts
{
    public class CreditAccount : AbstractAccount
    {
        private readonly IBank _bank;

        private decimal _balance;

        internal CreditAccount(IBank bank, IClient client, decimal balance)
            : base(client)
        {
            _bank = bank;
            Balance = balance;
        }

        public sealed override decimal Balance
        {
            get => _balance;
            set
            {
                if (value < Limit)
                    throw new BanksException($"Account balance can't be less than {Limit}");

                _balance = value;

                History.Add(new BalanceState(_balance));
            }
        }

        public decimal Comission =>
            _bank.CreditComission;

        public decimal Limit =>
            _bank.CreditLimit;

        internal override void AccrualOfPercents(BalanceState balanceState, int days)
        {
            try
            {
                if (balanceState.Balance < 0)
                    Balance -= Comission * days;
            }
            catch (BanksException)
            {
                Balance = Limit;
            }
        }
    }
}