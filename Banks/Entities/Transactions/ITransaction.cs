namespace Banks.Entities.Transactions
{
    public interface ITransaction
    {
        ITransaction Execute();

        void Cancel();
    }
}