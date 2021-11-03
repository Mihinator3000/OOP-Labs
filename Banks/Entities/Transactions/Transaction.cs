using Banks.Entities.Accounts;

namespace Banks.Entities.Transactions
{
    public class Transaction : ITransaction
    {
        private readonly AbstractAccount _sender;
        private readonly AbstractAccount _receiver;
        private readonly decimal _amount;

        private readonly Cancellable _cancellable = new ();

        public Transaction(AbstractAccount sender, AbstractAccount receiver, decimal amount)
        {
            _sender = sender;
            _receiver = receiver;
            _amount = amount;
        }

        public ITransaction Execute()
        {
            _sender.Balance -= _amount;
            _receiver.Balance += _amount;
            return this;
        }

        public void Cancel()
        {
            _cancellable.SetCancelled();
            new Transaction(_receiver, _sender, _amount).Execute();
        }
    }
}