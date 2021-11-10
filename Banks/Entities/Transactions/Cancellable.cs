using Banks.Tools;

namespace Banks.Entities.Transactions
{
    public class Cancellable
    {
        public bool Cancelled { get; private set; }

        public void SetCancelled()
        {
            if (Cancelled)
                throw new BanksException("Transaction is already cancelled");

            Cancelled = true;
        }
    }
}