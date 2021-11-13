using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions
{
    public class Withdrawal : ITransaction
    {
        private readonly Cancellable _cancellable = new ();

        public Withdrawal(AbstractAccount account, decimal amount)
        {
            Account = account;
            Amount = amount;
        }

        public AbstractAccount Account { get; }

        public decimal Amount { get; }

        public bool Cancelled =>
            _cancellable.Cancelled;

        public ITransaction Execute()
        {
            Account.Balance -= Amount;
            Account.Client.Balance += Amount;
            return this;
        }

        public void Cancel()
        {
            _cancellable.SetCancelled();
            new Replenishment(Account, Amount).Execute();
        }
    }
}