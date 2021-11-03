using System.Collections.Generic;
using Banks.Entities.Clients;
using Banks.Entities.Clients.Passport;

namespace Banks.Entities.Banks
{
    public interface IMainBank
    {
        List<IBank> Banks { get; }

        IBank RegisterBank(IBank bank);
        public IBank GetBank(int id);
        public void DeleteBank(IBank bank);

        public IClient AddClient(IClient client);
        public List<IClient> FindClients(string name, string surname);
        public IClient GetClient(PassportInfo passportInfo);
        public void DeleteClient(IClient client);

        public void AccrueOfPercents();
    }
}