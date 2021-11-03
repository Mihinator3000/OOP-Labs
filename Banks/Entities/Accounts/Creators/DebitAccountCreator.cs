using Banks.Entities.Banks;
using Banks.Entities.Clients;

namespace Banks.Entities.Accounts.Creators
{
    public class DebitAccountCreator : IAccountCreator
    {
        private readonly IBank _bank;

        public DebitAccountCreator(IBank bank)
        {
            _bank = bank;
        }

        public AbstractAccount Create(IClient client, decimal balance)
        {
            return new DebitAccount(_bank, client, balance);
        }
    }
}