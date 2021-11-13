using Banks.Entities.Accounts.Balance;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Tools;

namespace Banks.Entities.Accounts
{
    public class DebitAccount : AbstractAccount
    {
        private readonly IBank _bank;

        private decimal _balance;

        internal DebitAccount(IBank bank, IClient client, decimal balance)
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
                if (value < 0)
                    throw new BanksException("Account balance can't be less than 0");

                _balance = value;

                History.Add(new BalanceState(_balance));
            }
        }

        public decimal Interest =>
            _bank.DebitInterest;

        internal override void AccrualOfPercents(BalanceState balanceState, int days)
        {
            Balance += balanceState.Balance * Interest * days / 365;
        }
    }
}