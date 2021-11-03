using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions
{
    public class Withdrawal : ITransaction
    {
        private readonly AbstractAccount _account;
        private readonly decimal _amount;

        private readonly Cancellable _cancellable = new ();

        public Withdrawal(AbstractAccount account, decimal amount)
        {
            _account = account;
            _amount = amount;
        }

        public ITransaction Execute()
        {
            _account.Balance -= _amount;
            _account.Client.Balance += _amount;
            return this;
        }

        public void Cancel()
        {
            _cancellable.SetCancelled();
            new Replenishment(_account, _amount).Execute();
        }
    }
}