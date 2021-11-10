using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions
{
    public interface ITransaction
    {
        AbstractAccount Account { get; }

        decimal Amount { get; }

        bool Cancelled { get; }

        ITransaction Execute();

        void Cancel();
    }
}