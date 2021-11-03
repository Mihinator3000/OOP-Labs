using Banks.Entities.Clients;

namespace Banks.Entities.Accounts.Creators
{
    public interface IAccountCreator
    {
        AbstractAccount Create(IClient client, decimal balance);
    }
}