using Banks.Tools;

namespace Banks.Entities.Transactions
{
    public class Cancellable
    {
        private bool _cancelled;

        public void SetCancelled()
        {
            if (_cancelled)
                throw new BanksException("Transaction is already cancelled");

            _cancelled = true;
        }
    }
}