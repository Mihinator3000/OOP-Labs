using System;
using Banks.Services.Time;

namespace Banks.Entities.Accounts.Balance
{
    public class BalanceState
    {
        public BalanceState(decimal balance)
        {
            Balance = balance;
            Time = GlobalTime.Now;
        }

        public decimal Balance { get; }

        public DateTime Time { get; }
    }
}