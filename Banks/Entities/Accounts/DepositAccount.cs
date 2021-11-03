using Banks.Entities.Accounts.Balance;
using Banks.Entities.Clients;
using Banks.Tools;

namespace Banks.Entities.Accounts
{
    public class DepositAccount : AbstractAccount
    {
        private decimal _balance;

        internal DepositAccount(IClient client, decimal balance)
            : base(client)
        {
            Balance = balance;
            Interest = GetInterect(balance);
        }

        public sealed override decimal Balance
        {
            get => _balance;
            set
            {
                if (value < _balance)
                    throw new BanksException("Account is not withdrawable");

                _balance = value;

                History.Add(new BalanceState(_balance));
            }
        }

        public decimal Interest { get; }

        internal override void AccrualOfPercents(BalanceState balanceState, int days)
        {
            Balance += balanceState.Balance * Interest * days / 365;
        }

        private static decimal GetInterect(decimal amount)
        {
            return amount < 50000 ? 3
                : (amount < 100000) ? 3.5M : 4;
        }
    }
}