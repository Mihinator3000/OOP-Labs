using Banks.Entities.Banks;
using Banks.Entities.Clients;

namespace Banks.Entities.Accounts.Creators
{
    public class CreditAccountCreator : IAccountCreator
    {
        private readonly IBank _bank;

        public CreditAccountCreator(IBank bank)
        {
            _bank = bank;
        }

        public AbstractAccount Create(IClient client, decimal balance)
        {
            return new CreditAccount(_bank, client, balance);
        }
    }
}