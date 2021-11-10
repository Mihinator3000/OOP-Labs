using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions
{
    public class Replenishment : ITransaction
    {
        private readonly Cancellable _cancellable = new ();

        public Replenishment(AbstractAccount account, decimal amount)
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
            Account.Client.Balance -= Amount;
            Account.Balance += Amount;
            return this;
        }

        public void Cancel()
        {
            _cancellable.SetCancelled();
            new Withdrawal(Account, Amount).Execute();
        }
    }
}