using System.Collections.Generic;
using Banks.Entities.Accounts.Balance;
using Banks.Entities.Clients;

namespace Banks.Entities.Accounts
{
    public abstract class AbstractAccount
    {
        protected AbstractAccount(IClient client)
        {
            Client = client;
        }

        public IClient Client { get; }

        public abstract decimal Balance { get; set; }

        public bool NotifyClient { get; set; }

        internal List<BalanceState> History { get; } = new ();

        internal abstract void AccrualOfPercents(BalanceState balanceState, int days);
    }
}