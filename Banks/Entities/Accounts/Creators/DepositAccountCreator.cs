using Banks.Entities.Clients;

namespace Banks.Entities.Accounts.Creators
{
    public class DepositAccountCreator : IAccountCreator
    {
        public AbstractAccount Create(IClient client, decimal balance)
        {
            return new DepositAccount(client, balance);
        }
    }
}