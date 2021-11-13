using Banks.Entities.Accounts;
using Banks.Tools;

namespace Banks.Entities.Transactions
{
    public class Transaction : ITransaction
    {
        private readonly Cancellable _cancellable = new ();

        public Transaction(AbstractAccount sender, AbstractAccount receiver, decimal amount)
        {
            Account = sender;
            Receiver = receiver;
            Amount = amount;
        }

        public AbstractAccount Account { get; }

        public AbstractAccount Receiver { get; }

        public decimal Amount { get; }

        public bool Cancelled =>
            _cancellable.Cancelled;

        public ITransaction Execute()
        {
            if (Account == Receiver)
                throw new BanksException("Can not transact to sender");

            Account.Balance -= Amount;
            Receiver.Balance += Amount;
            return this;
        }

        public void Cancel()
        {
            _cancellable.SetCancelled();
            new Transaction(Receiver, Account, Amount).Execute();
        }
    }
}