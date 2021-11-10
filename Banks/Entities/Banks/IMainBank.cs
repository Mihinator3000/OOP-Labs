using System.Collections.Generic;
using Banks.Entities.Clients;
using Banks.Entities.Clients.Passport;

namespace Banks.Entities.Banks
{
    public interface IMainBank
    {
        List<IBank> Banks { get; }

        IBank RegisterBank(IBank bank);
        IBank GetBank(int id);
        void DeleteBank(IBank bank);

        IClient AddClient(IClient client);
        List<IClient> FindClients(string name, string surname);
        IClient GetClient(PassportInfo passportInfo);
        void DeleteClient(IClient client);

        void AccruePercentage();
    }
}