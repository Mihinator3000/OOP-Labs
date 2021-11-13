using System;
using Banks.Services.Time;

namespace Banks.Entities.Accounts.Balance
{
    internal class BalanceState
    {
        internal BalanceState(decimal balance)
            : this(balance, GlobalTime.Now)
        {
        }

        internal BalanceState(decimal balance, DateTime time)
        {
            Balance = balance;
            Time = time;
        }

        internal decimal Balance { get; }

        internal DateTime Time { get; }
    }
}